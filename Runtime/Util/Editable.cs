using System;

namespace ch.sycoforge.Util.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                               System.AttributeTargets.Property | System.AttributeTargets.Field,
                               AllowMultiple = true)]
    public class Editable : System.Attribute
    {

        public enum EditType { STANDARD, READONLY, HIDDEN, RESOURCE };
        public enum EditFormat { Normal, Percent, Rounded };

        //-------------------------------------
        // Properties
        //-------------------------------------

        public string ClassPath
        {
            get
            {
                return classPath;
            }

            set
            {
                classPath = value;
            }
        }

        public string AliasPath
        {
            get
            {
                return aliasPath;
            }

            set
            {
                aliasPath = value;
            }
        }

        /// <summary>
        /// Defines if this is a referenceable Property
        /// </summary>
        public bool IsResource
        {
            get
            {
                return editType == EditType.RESOURCE;
            }
        }

        /// <summary>
        /// The name of the referencable Property.
        /// <see cref="IsResource"/>
        /// </summary>
        public string ReferenceName
        {
            get;
            set;
        }

        /// <summary>
        /// Tooltip displayed when hovering name label.
        /// </summary>
        public string Tooltip
        {
            get;
            set;
        }

        public bool Validatable
        {
            get;
            set;
        }

        public bool DynamicColletion
        {
            get;
            set;
        }

        public EditFormat Format
        {
            get;
            set;
        }

        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Is the range between [0..1].
        /// </summary>
        public bool Dynamic
        {
            get { return normalized; }
            set { normalized = value; }
        }

        //public bool CurveInput
        //{
        //    get;
        //    set;
        //}

        public bool IsReadOnly
        {
            get
            {
                return editType == EditType.READONLY;
            }
        }

        public bool HasSpecialFormat
        {
            get
            {
                return Format != EditFormat.Normal;
            }
        }

        public float Min
        {
            get { return min; }
            set { min = value; HasRange = true; }
        }

        public float Max
        {
            get { return max; }
            set { max = value; HasRange = true; }
        }

        public float MinX
        {
            get { return minX; }
            set { minX = value; }
        }

        public float MaxX
        {
            get { return maxX; }
            set { maxX = value; }
        }

        public string Group
        {
            get { return group; }
            set { group = value; }
        }

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        public EditType Type
        {
            get
            {
                return editType;
            }
        }

        public Type TargetType
        {
            get
            {
                return type;
            }
        }

        public string VisibiltyDependsOn
        {
            get;
            set;
        }

        public bool VisibiltyDepended
        {
            get
            {
                return !string.IsNullOrEmpty(VisibiltyDependsOn);
            }
        }

        public bool HasAlias
        {
            get
            {
                return alias != null && alias != string.Empty;
            }
        }

        public int Indent
        {
            get;
            set;
        }

        //---------------------------------------
        // Fields
        //---------------------------------------
        public bool HasRange = false;

        private EditType editType = EditType.STANDARD;

        private string name = null, classPath = string.Empty, aliasPath = string.Empty, alias = string.Empty;
        //private bool isValidatable = false;
        private System.Collections.IEnumerable multiData = null;
        private float min, max, minX = 0, maxX = 1;
        private string group = string.Empty;
        private Type type;
        private bool normalized;
        //-------------------------------------
        // Constructor
        //-------------------------------------

        public Editable()
        {

        }

        public Editable(EditType editType)
        {
            this.editType = editType;
        }

        public Editable(EditType editType, Type type)
        {
            this.editType = editType;
            this.type = type;
        }


        //-------------------------------------
        // Public Static Methods
        //-------------------------------------

        public object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        //public System.Collections.IEnumerable FindItemSource(string ClassPath)
        //{
        //    string[] pathValues = ClassPath.Split(':');

        //    Assembly assembly = null;
        //    string AssemblyPath = pathValues[0];
        //    string ClassFullname = pathValues[1];
        //    string PropertyName = pathValues[2];


        //    assembly = Assembly.LoadFrom(AssemblyPath);


        //    Type[] types = assembly.GetTypes();
        //    Type singletonType = null;

        //    foreach (Type ty in types)
        //    {
        //        if (ty.FullName.Equals(ClassFullname))
        //        {
        //            singletonType = ty;
        //            break;
        //        }
        //    }

        //    if (singletonType == null)
        //    {
        //        return null;
        //    }


        //    PropertyInfo property = singletonType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);

        //    object instance = property.GetValue(null, null);

        //    object value = GetPropValue(instance, PropertyName);



        //    return (System.Collections.IEnumerable)value;
        //    //return Convert.ChangeType(value, Convert.GetTypeCode(value));
        //}

        //-------------------------------------
        // Public Methods
        //-------------------------------------

        //public System.Collections.IEnumerable GetMultiData()
        //{
        //    if (ClassPath != null)
        //    {
        //        multiData = FindItemSource(ClassPath);

        //        return multiData;
        //    }

        //    return multiData;
        //}
    }
}
