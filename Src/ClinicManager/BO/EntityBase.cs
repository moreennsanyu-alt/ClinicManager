using System;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace TestValidation.Module {
    [DomainComponent, DefaultClassOptions, ImageName("BO_Employee")]
    public interface IEmployee {
        [RuleRequiredField(EmployeeValidationRules.EmployeeNameIsRequired, DefaultContexts.Save)]
        string Name { get; set; }
        [RuleValueComparison(EmployeeValidationRules.EmployeeIsAdult, DefaultContexts.Save, 
            ValueComparisonType.GreaterThanOrEqual, 18)]
        int Age { get; set;}
    }
    public class EmployeeValidationRules {
        public const string EmployeeNameIsRequired = "EmployeeNameIsRequired";
        public const string EmployeeIsAdult = "EmployeeIsAdult";
    }

}
