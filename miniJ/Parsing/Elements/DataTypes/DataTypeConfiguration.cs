namespace miniJ.Parsing.Elements.DataTypes
{
    public struct DataTypeConfiguration
    {
        public bool Array;
        public bool Constant;
        public bool Volatile;
        public bool ReadOnly;
        public bool Static;

        DataTypeConfiguration(bool array = false, bool constant = false, bool @volatile = false, bool readOnly = false, bool @static = false)
        {
            Array = array;
            Constant = constant;
            Volatile = @volatile;
            ReadOnly = readOnly;
            Static = @static;
        }
    }
}
