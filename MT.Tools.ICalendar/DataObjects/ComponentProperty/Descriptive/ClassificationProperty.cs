using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using MT.Tools.ICalendar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public enum ClassificationType
    {
        Public,
        Private,
        Confidential,
        Custom
    }

    public class ClassificationProperty : IComponentProperty
    {
        #region Constructor

        public ClassificationProperty() { }

        public ClassificationProperty(ClassificationValue classification) { Classification = classification; }

        public ClassificationProperty(ClassificationValue classification, IEnumerable<IPropertyParameter> parameters) { Classification = classification; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.Classification;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public ClassificationValue Classification { get; set; } = ClassificationValue.PUBLIC;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with CLASS
            if (!content.ToUpper().StartsWith("CLASS")) { throw new ArgumentException("Invalid categories detected! Component property needs to start with CLASS keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("CLASS".Length, content.IndexOf(':') - "CLASS".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            Classification = ObjectSerializer.Deserialize<ClassificationValue>(valueContent);
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"CLASS{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Classification }";
        }

        #endregion Methods
    }

    public class ClassificationValue : IPropertyValue
    {
        #region Constants

        public const string CLASSIFICATION_PUBLIC = "PUBLIC";
        public const string CLASSIFICATION_PRIVATE = "PRIVATE";
        public const string CLASSIFICATION_CONFIDENTIAL = "CONFIDENTIAL";

        private static readonly Dictionary<ClassificationType, string> CLASSIFICATION_STRINGS = new Dictionary<ClassificationType, string>()
        {
            {  ClassificationType.Public,       CLASSIFICATION_PUBLIC       },
            {  ClassificationType.Private,      CLASSIFICATION_PRIVATE      },
            {  ClassificationType.Confidential, CLASSIFICATION_CONFIDENTIAL },
        };

        public static readonly ClassificationValue PUBLIC = new ClassificationValue(CLASSIFICATION_PUBLIC);
        public static readonly ClassificationValue PRIVATE = new ClassificationValue(CLASSIFICATION_PRIVATE);
        public static readonly ClassificationValue CONFIDENTIAL = new ClassificationValue(CLASSIFICATION_CONFIDENTIAL);

        #endregion Constants

        #region Constructor

        public ClassificationValue() { }

        //public ClassificationValue(ClassificationType standardClassification)
        //{
        //    // make sure that the classification is not custom
        //    if (ClassificationType == ClassificationType.Custom) { throw new ArgumentException("Invalid type detected! Please use the constructor for custom classifications!"); }

        //    ClassificationText = CLASSIFICATION_STRINGS[standardClassification];
        //}

        public ClassificationValue(string customClassification) { ClassificationText = customClassification; }

        #endregion Constructor

        public PropertyValueType Type => PropertyValueType.Text;

        public ClassificationType ClassificationType => 
            CLASSIFICATION_STRINGS.ContainsValue(ClassificationText) ? CLASSIFICATION_STRINGS.InversePairs()[ClassificationText] : ClassificationType.Custom;

        public string ClassificationText { get; private set; } = null;

        public void Deserialize(string content)
        {
            ClassificationText = content.Trim();
        }

        public string Serialize()
        {
            return $"{ ClassificationText }";
        }
    }
}
