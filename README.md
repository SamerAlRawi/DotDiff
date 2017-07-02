# DotDiff
Object comparison and audit library for .net

### Examples:


[Using Expressions](#expressions)

[Using Attributes](#attributes)

[Manually Adding KeyPairs](#keypairs)

#### <a name="expressions"></a>Expressions Examples

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


For Xml serialization add the following line to your Program.cs main:
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

For Json serialization add the following line to your Program.cs main:
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

###
###
#### <a name="attributes"></a>Attributes Examples
Properties annotated with [Audit] attributes will be tracked for auditing by default.
Create a new console application and install the following nuget package
```
Install-Package DotDiff
```
Create Class 
```csharp
public class User
{
    [Audit]
    public string Email { get; set; }
    public string UserName { get; set; }
}
```


For Xml serialization add the following line to your Program.cs main:
```csharp
var user1 = new User{
    Email = "m1@domain1.com",
    UserName = "user1"
};
var user2 = new User{
    Email = "m2@domain2.com",
    UserName = "user2"
};

var xml = new XmlAuditBuilder<User>()
                .Audit(user1, user2)
                .Serialize();
            ForegroundColor = ConsoleColor.Green;
            WriteLine(xml);
```
The result will include the email values by default:
```xml
<ArrayOfAuditPair>
  <AuditPair>
    <Key>Email</Key>
    <OldValue>m1@domain1.com</OldValue>
    <NewValue>m2@domain2.com</NewValue>
  </AuditPair>
</ArrayOfAuditPair>
```

For Json serialization add the following line to your Program.cs main:
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



###
###
#### <a name="keypairs"></a>Key Pairs Example

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

###
###
### Future work:
- collections 
- nested properties and classes