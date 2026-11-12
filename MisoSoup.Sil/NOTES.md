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

