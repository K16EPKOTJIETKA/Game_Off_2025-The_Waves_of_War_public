

public static class SignalFormulaFactory
{
    public static ISignalFormula GetFormula(Formula formula)
    {
        switch (formula)
        {
            case Formula.AmplitudeModulation:
                return new AMFormula();
               
            case Formula.FrequencyModulation:
                return new FMFormula();
               
            case Formula.FrequencyShiftKeying:
                return new FSKFormula();
               
            case Formula.QuadratureAmplitudeModulation:
                return new QAMFormula();
           
            default:
                return new AMFormula();
        }
    }
}