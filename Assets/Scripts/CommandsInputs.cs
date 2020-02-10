
public enum CommandsInputsEnum
{
    NONE,
    RIGHT,
    LEFT,
    JUMP,
    INTERACT,
    START,
    END
}

public class CommandsInputs
{
    public CommandsInputsEnum ci;
    public float time;    

    public CommandsInputs(CommandsInputsEnum cie, float mytime)
    {
        ci = cie;
        time = mytime;
    }
}
