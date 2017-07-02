# DotDiff
Object comparison and audit library for .net

### Examples:
Create a new console application and install the following nuget package
```
Install-Package DotDiff
```
Create Class 
```csharp
public class User
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool Enabled { get; set; }
    public long Id { get; set; }
    public DateTime LastLogin { get; set; }
}
```


For Xml serialization add the following live to your Program.cs main:
```csharp
var user1 = new User{
    //set all properties here
};
var user2 = new User{
    //set all properties here
};

var xml = new XmlAuditBuilder<User>()
                .Audit(user1, user2)
                .Include(_ => _.Email)
                .Include(_ => _.Password)
                .Include(_ => _.UserName)
                .Include(_ => _.Id)
                .Include(_ => _.Enabled)
                .Include(_ => _.LastLogin)
                .Serialize();
            ForegroundColor = ConsoleColor.Green;
            WriteLine(xml);
```

For manually added attributes you can use the auditpair overload:
```csharp
    var xml = new XmlAuditBuilder<User>()
                .Audit(user1, user2)
                .Include(_ => _.Id)
                .Include(
                    new AuditPair{
                        Key = "OtherAttribute123",
                        OldValue = "any value",
                        NewValue = null //or any other value if needed  
                    }
                )
                .Serialize();
```


For Json serialization add the following live to your Program.cs main:
```csharp
var user1 = new User{
    //set all properties here
};
var user2 = new User{
    //set all properties here
};

var json = new JsonAuditBuilder<User>()
                .Audit(user1, user2)
                .Include(_ => _.Email)
                .Include(_ => _.Password)
                .Include(_ => _.UserName)
                .Include(_ => _.Id)
                .Include(_ => _.Enabled)
                .Include(_ => _.LastLogin)
                .Serialize();
            ForegroundColor = ConsoleColor.Green;
            WriteLine(json);
```

### Future work:
- collections 
- nested properties and classes