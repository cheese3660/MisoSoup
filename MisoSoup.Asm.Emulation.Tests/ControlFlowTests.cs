namespace MisoSoup.Asm.Emulation.Tests;

public class ControlFlowTests
{
    [Test]
    public void NopDoesNothing()
    {
        
        var program = new Program();
        program.Mark("main");
        program.Emit(new Nop());
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        // Check that literally nothing changed
        using (Assert.EnterMultipleScope())
        {
            for (var i = 0; i < 32; i++)
            {
                Assert.That(emulator.GetRegisterS(new Register(RegisterBank.Saved, (byte)i)), Is.EqualTo(0));
                if (i >= 16) continue;
                Assert.That(emulator.GetRegisterS(new Register(RegisterBank.Argument, (byte)i)), Is.EqualTo(0));
                Assert.That(emulator.GetRegisterS(new Register(RegisterBank.Temporary, (byte)i)), Is.EqualTo(0));
            }
        }
    }

    [Test]
    public void CallAndReturnWorks()
    {
        var program = new Program();
        program.Mark("target");
        program.Emit(new Move(Register.a0, 1));
        program.Emit(new Return());
        program.Mark("main");
        program.Emit(new Call(new DefinedOperand("target")));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(4, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
    [Test]
    public void CallPaAndReturnWorks()
    {
        var register = new Register(RegisterBank.Temporary, 0);
        var program = new Program();
        program.Mark("target");
        program.Emit(new Move(Register.a0, 1));
        program.Emit(new Return());
        program.Mark("main");
        program.Emit(new Move(register, new DefinedOperand("target")));
        program.Emit(new CallPa(new RegisterOperand(register)));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(4, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
    [Test]
    public void JumpWorks()
    {
        var program = new Program();
        program.Mark("jump_target");
        program.Emit(new Move(Register.a0, 1));
        program.Emit(new Exit());
        
        program.Mark("main");
        program.Emit(new Jump("jump_target"));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
    
    [Test]
    public void JumpPaWorks()
    {
        var program = new Program();
        program.Mark("jump_target");
        program.Emit(new Move(Register.a0, 1));
        program.Emit(new Exit());
        program.Mark("main");
        program.Emit(new Move(Register.t0, "jump_target"));
        program.Emit(new JumpPa(Register.t0));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
    [Test]
    public void JumpTrueJumpsWhenTrue()
    {
        var program = new Program();
        program.Mark("target");
        program.Emit(new Move(Register.a0, 1));
        program.Emit(new Exit());
        program.Mark("main");
        program.Emit(new Move(Register.cr, "true"));
        program.Emit(new JumpTrue("target"));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
    [Test]
    public void JumpTrueDoesNotJumpWhenFalse()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new JumpTrue("target"));
        program.Emit(new Move(Register.a0, 1));
        program.Mark("target");
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
    [Test]
    public void JumpTruePaJumpsWhenTrue()
    {
        var program = new Program();
        program.Mark("target");
        program.Emit(new Move(Register.a0, 1));
        program.Emit(new Exit());
        program.Mark("main");
        program.Emit(new Move(Register.t0, "target"));
        program.Emit(new Move(Register.cr, "true"));
        program.Emit(new JumpTruePa(Register.t0));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
    [Test]
    public void JumpTruePaDoesNotJumpWhenFalse()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.t0, "target"));
        program.Emit(new JumpTruePa(Register.t0));
        program.Emit(new Move(Register.a0, 1));
        program.Mark("target");
        program.Emit(new Exit());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }

    [Test]
    public void JumpFalseJumpsWhenFalse()
    {
        var program = new Program();
        program.Mark("target");
        program.Emit(new Move(Register.a0, 1));
        program.Emit(new Exit());
        program.Mark("main");
        program.Emit(new JumpFalse("target"));
        program.Mark("exit");
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }

    [Test]
    public void JumpFalseDoesNotJumpWhenTrue()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.cr, "true"));
        program.Emit(new JumpFalse("target"));
        program.Emit(new Move(Register.a0, 1));
        program.Mark("target");
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
    

    [Test]
    public void JumpFalsePaJumpsWhenFalse()
    {
        var program = new Program();
        program.Mark("target");
        program.Emit(new Move(Register.a0, 1));
        program.Emit(new Exit());
        program.Mark("main");
        program.Emit(new Move(Register.t0, "target"));
        program.Emit(new JumpFalsePa(Register.t0));
        program.Mark("exit");
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }

    [Test]
    public void JumpFalsePaDoesNotJumpWhenTrue()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.t0, "target"));
        program.Emit(new Move(Register.cr, "true"));
        program.Emit(new JumpFalsePa(Register.t0));
        program.Emit(new Move(Register.a0, 1));
        program.Mark("target");
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(1));
    }
    
}