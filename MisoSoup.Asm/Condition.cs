namespace MisoSoup.Asm;

public enum Condition
{
    Equal,
    NotEqual,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThanUnsigned,
    GreaterThanUnsigned,
    LessThanEqualUnsigned,
    GreaterThanEqualUnsigned,
    FloatEqualApproximate,
    FloatNotEqualApproximate,
    FloatLessThan,
    FloatGreaterThan,
    FloatNaN,
    FloatInf
}

public static class ConditionExtensions
{
    public static string AsString(this Condition condition) => condition switch
    {
        Condition.Equal => "eq",
        Condition.NotEqual => "neq",
        Condition.LessThan => "lt",
        Condition.LessThanOrEqual => "lte",
        Condition.GreaterThan => "gt",
        Condition.GreaterThanOrEqual => "gte",
        Condition.LessThanUnsigned => "ltu",
        Condition.GreaterThanUnsigned => "gtu",
        Condition.LessThanEqualUnsigned => "lteu",
        Condition.GreaterThanEqualUnsigned => "gteu",
        Condition.FloatEqualApproximate => "feqa",
        Condition.FloatNotEqualApproximate => "fneqa",
        Condition.FloatLessThan => "flt",
        Condition.FloatGreaterThan => "fgt",
        Condition.FloatNaN => "fnan",
        Condition.FloatInf => "finf",
        _ => throw new ArgumentOutOfRangeException(nameof(condition), condition, null)
    };
}