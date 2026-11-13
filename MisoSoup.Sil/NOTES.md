# Miso Soup Stack Intermediary Language (SIL)

SIL is an in memory intermediary representation of Miso Soup programs, and it is targeted *mainly* for the
MISA architecture, but it is eventually meant to be for architectures generally.

It needs to support most things a program can do, and structured types, and it will be flavored based off of CIL

There will be a textual format, and that is what will be used to describe it here

## Types

SIL supports a wide range of types necessary for describing almost any program that could run (or not run)
on MISA

### Basic Primitives

| Type Name | Description                                                              |
|-----------|--------------------------------------------------------------------------|
| void      | An empty type                                                            |
| bool      | true/false value (in MISA this will generally go into the `cr` register) |
| i8        | Signed 8 bit value                                                       |
| u8        | Unsigned 8 bit vlaue                                                     |
| i16       | Signed 16 bit value                                                      |
| u16       | Unsigned 16 bit value                                                    |
| i32       | Signed 32 bit value (a native int)                                       |
| u32       | Unsigned 32 bit value                                                    |
| i64       | Signed 64 bit value                                                      |
| u64       | Unsigned 64 bit value                                                    |
| f32       | 32 bit floating point number                                             |
| f64       | 64 bit floating point number (unsupported on MISA)                       |
| ptr       | Pointer to an address in memory                                          |

### Aggregate Types

#### Vectors

SIL natively supports vectors of primitive types, a vector is represented as `<primitive:N>`, the maximum natively
supported value of N depends on the architecture, for MISA it is 32 (though with a recommended value of 16)

Vectors function as primitive types for most cases, how these get handled on the lowering side is up to the lowerer,
though generally they should be handled as native vector instructions.

Examples

```
<f32:16> - 16 element float vector
<u8:3> - 3 element u8 vector (for a possible example - a color)
```

#### Arrays

SIL supports array types in data/struct declarations and as locals/parameters, they are represented as `[other:N]`

Examples

```
[f32:8] - 8 element float array
[ptr:64] - 64 element pointer array
[[ptr:32]:8] - 8 row by 32 colum pointer array
```

#### Bitsets

SIL natively supports bitsets, which are larger integers spliced into multiple fields, the representation is
`bitset{<integer>:fields..}`

fields start from the low bit to the high bit and are defined as such
b - 1 bit boolean flag
uN - N bit unsigned number
sN - N bit signed number
pN - N bits of padding

So like the following bitset stores an RGBA value inside an u32
`bitset{u32:u8,u8,u8,u8}`

Bitsets are represented generally with a `typeid` rather than showing the full bitset type everywhere

#### Structures

SIL natively supports structures, which are the basic aggregate type, the representation is
`struct <attributes> {type..}`

Structures can have attributes like `.packed` to denote that they are stored in a packed representation in memory rather
than aligned

(Structs are given a `typeid` to deduplicate them internally, and when dumped they are referred to by their type id)

#### Unions

SIL natively supports unions, the representation is
`union {type..}`

(Unions are given a `typeid` to deduplicate them internally, and when dumped they are referred to by their type id)

#### Type ID declarations

Type IDs as mentioned before are shown in the representation as

```
.type `ID = X
```

## Global Declarations

SIL allows for the declaration of global values - either as a target specific embed (like for MISA), a list of values,
or just a reserved space. All of these eventually become pointers or *maybe* registers (depending on the target, though
they can have the attribute of being a register - for MISA these will go in the `s` registers with up to the 16 upper
registers allowed to be allocated.

### Variables

```
.global <attributes> <type>? @<id> "<name>" = <optional initializer>; Reserve a global with an optional name, if the type is unspecified, then the global is only referred to as a pointer and read as such
.global i32 @0 "something" = 5i32 ; Reserve a global with a defined value
.type `0 = struct { i32, i32 }
.global `0 @1 = { 5i32, 6i32 } ; Reserve a global with a defined structure value
.type `1 = union { `5, ptr }
.global `1 @2 = { ptr = @1 + 2 } ; Reserve a global with a defined union value, and a pointer directly to `@1` (offset by 2 bytes, which is allowed)
.global [u8:6] @3 "string" = .asciiz "hello" ; Reserve a null terminated string (of an implied length)
.global [u8:?] @4 "file" = .embed "file.bin" ; Define a global that is a u8 array of a specified length (that the compiler must know)
.global void @5 = .embed "file.png" ; Define a global as `void`
.global ptr @6 "image" = @5 ; Then point to the type erased data (not implemented yet)
```

#### Attributes

| Attribute   | Description                                                                              |
|-------------|------------------------------------------------------------------------------------------|
| `.extern`   | States that this variable is externally defined, hence why there is no definition for it |
| `.export`   | Exports this variable (only used on some targets, not MISA)                              |
| `.register` | States that this variable should be reserved into a globally kept register               |

### Functions

A function has a return type, an ID, an optional name, and a list of parameter types, as well as a list of locals

They are represented as such:

```
.function <attributes> @<id> "<name>" (<param types...>) <return_type> /* optional body follows*/ [ <variable_types> ]
{
    /* All the IL instructions in the function follow */
}
```

#### Attributes

| Attribute    | Description                                                                            |
|--------------|----------------------------------------------------------------------------------------|
| `.entry`     | Defines this function as an "entrypoint" which on MISA corresponds to processes        |
| `.noreturn`  | States that this function will not return, allowing the target to optimize calls to it |
| `.extern`    | States that this function is externally defined, hence why it's body is empty          |
| `.export`    | Exports this function (only used on some targets, not MISA)                            |
| `.intrinsic` | The given function is actually an intrinsic function, determined by it's name          |

## Intermediary Language

This defines the actual instructions that go into a methods body.

### Marks

Before we get into the actual instructions, we need to get into `marks` first which are basically the IL version of
labels. They basically are pointers to the `Nth` instruction. They are represented in SIL as follows

```
<ID>: (relevant instruction)
```

then later referred to via `ID`

### Basic Data Transfer Instructions

SIL has a lot of basic data moving instructions for manipulating data on the stack, including loading/storing to/from
all 3 different types of parameters, loading from structure fields and storing into structure fields, same with arrays.
Then for reading and writing into vectors as well.

For reference, anything that has a `.x` form actually has 3 forms with different first arguments:

- `.a N` (for the nth argument), though note these are immutable
- `.l N` (for the nth local)
- `.g @<id> "<name>"` (for the given global)
  Each of these will be referred to as the referent

| Instruction                     | Description                                                                                                          |
|---------------------------------|----------------------------------------------------------------------------------------------------------------------|
| `dup`                           | Duplicate the top value on the stack                                                                                 |
| `discard`                       | Discard the top value on the stack                                                                                   |
| `swap`                          | Swap the top 2 values on the stack                                                                                   |
| `take`                          | Take the Nth value from the stack                                                                                    |
| `ld.x`                          | Push the referant to the stack                                                                                       |
| `st.x`                          | Pop the top value on the stack into the referent                                                                     |
| `addr.x`                        | Push the address of the referent onto the stack                                                                      |
| `write`                         | Store the top value on the stack into the pointer at the 2nd top of the stack                                        |
| `read <type>`                   | Pop the pointer at the top of the stack and load it as the given type                                                |
| `ldfield.x <field chain>`       | Load the pointed to field from the referent                                                                          |
| `stfield.x <field_chain>`       | Store the top value on the stack to the pointed to field from the referent                                           |
| `fieldaddr.x <field_chain>`     | Get the address of the pointed to field of the referent                                                              |
| `ldarr.x`                       | Load the index pointed to by the top value of the stack from the referent array                                      |
| `starr.x`                       | Store the top value of the stack into the index pointed to by the 2nd top value of the stack into the referent array |
| `fieldoff <type> <field_chain>` | Offset the top pointer by the given field offset, assuming it to be a pointer to the given type                      |
| `arroff <type>`                 | Offset second to top pointer by the top array index, assuming it to be a pointer to the given type (as an array)     |
| `ldvec.x N`                     | Load the Nth field of the vector referent on it's own                                                                |
| `stvec.x N`                     | Store the Nth field of the vector referent on it's own                                                               |
| `vecf N`                        | Load the Nth field of the vector on the top of the stack                                                             |
| `veci N`                        | Insert the top value on the stack into the vector at the Nth top of the stack                                        |
| `vecoff <type> N`               | Offset the top pointer by the given vector index, assuming it to be a pointer to a vector of the given type          |
| `ldswizzle.x <swizzle fields>`  | Load the given fields of the referent vector as a swizzle into their own vector on the stack                         |
| `stswizzle.x <swizzle fields>`  | Store the top of the stack as swizzle fields to the referent vector                                                  |
| `getswizzle <swizzle fields>`   | Get the swizzle fields of the vector at the top of the stack                                                         |
| `setswizzle <swizzle fields>`   | Set the swizzle fields in the vector at the second top of the stack to the vector at the top of the stack            |
| `ldbitfield.x N`                | Read the Nth bitfield from the referent                                                                              |
| `stbitfield.x N`                | Store the top value on the stack into the Nth bitfield of the referent                                               |

### Basic Data Loading And Conversion Instructions

| Instruction          | Description                                                          |
|----------------------|----------------------------------------------------------------------|
| `true`               | Push true onto the stack                                             |
| `false`              | Push false onto the stack                                            |
| `<primitive type> N` | Push a primitive type value onto the stack                           |
| `splat.x`            | Splat the value at the top of the stack into the referent vector     |
| `expand N`           | Expand the value at the top of the stack into a vector of length N   |
| `to <type>`          | Cast the top value on the stack into the given primitive/vector type |

### Basic control flow instructions

| Instruction                               | Description                                                                                                                                                      |
|-------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `nop`                                     | Do nothing                                                                                                                                                       |
| `j N`                                     | Immediately transfer control to the relevant mark                                                                                                                |
| `j.t N`                                   | Jump to the given mark if the top of the stack is true                                                                                                           |
| `j.f N`                                   | Jump to the given mark if the top of the stack is false                                                                                                          |
| `cal.g @<id> "name"`                      | Call the given global                                                                                                                                            |
| `cal.p (<parameter types>) <return_type>` | Call the pointer at the (parameter type count + 1) position with all the parameters in order from N to 1 on the stack and push the returned value onto the stack |
| `ret`                                     | Return from a function                                                                                                                                           |

On MISA, structures/unions/arrays on the stack typically are referred to via pointers, but that will not show in the IL
where they just can go on the stack (though, they should necessarily be copied on the callee side if they get written
too at all)

