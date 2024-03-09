using System.ComponentModel.DataAnnotations;
using SMART.Common.CompanyManagement;
using SMART.Common.ProjectManagement;
using SMART.Common.Base;
namespace SO.Data;

public class ProjectFormView : SMARTBaseClass { 

    public ProjectFormView(string LinkID) : base(LinkID) { }

    [Required]
    [StringLength(30, ErrorMessage = "Id too long (30 character limit).")]
    public string Name { get; set; } = default!;
   
    [Required]
    public bool IsShipped { get; set; } = false;
    public string LinkIDEmployeeSoldBy { get; set; } = default!;
    
    [RequiredIf(
        "IsShipped", 
        true, 
        ErrorMessage = "Shipping Location is required when IsShipped is true."
    )]
    public ShipLocation ProjectShipLocation { get; set; } = default!;

    public IList<ProjectGroup> ProjectGroups { get; set; } = default!;
}

public class RequiredIfAttribute : ValidationAttribute {
    private readonly string _propertyName;
    private readonly object _expectedValue;

    public RequiredIfAttribute(string propertyName, object expectedValue) {
        _propertyName = propertyName;
        _expectedValue = expectedValue;
    }

    protected override ValidationResult? IsValid(
        object? value, 
        ValidationContext validationContext
    ) {
        var instance = validationContext.ObjectInstance;
        var type = instance.GetType();
        var propertyValue = type?.GetProperty(_propertyName)?.GetValue(instance, null);

        if (propertyValue?.ToString() == _expectedValue.ToString() && value == null) {
            var memberNames = new List<string>();
            if (validationContext.MemberName != null) {
                memberNames.Add(validationContext.MemberName);
            } else {
                Console.WriteLine("ValidationContext.MemberName is null");
            }
            return new ValidationResult(ErrorMessage, memberNames);
        }
        return ValidationResult.Success;
    }
}
