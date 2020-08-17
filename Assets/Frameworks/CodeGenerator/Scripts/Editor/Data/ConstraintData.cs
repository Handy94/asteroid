namespace HandyPackage.CodeGeneration
{
    /// <summary> Constraints are stuff like <T1> and <T2> where T2 : ConstraintClass
    /// In this case, T1 is the constraint identifier while ConstraintClass in the ConstraintBaseType.
    /// </summary>
    public class ConstraintData
    {
        public string m_ConstraintIdentifier;
        public string m_ConstraintBaseType;

        public ConstraintData(string identifier, string baseType)
        {
            m_ConstraintIdentifier = identifier;
            m_ConstraintBaseType = baseType;
        }
    }
}

