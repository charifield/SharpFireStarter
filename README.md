# SharpFireStarter

![Pass](https://camo.githubusercontent.com/5b9a317a67c1a001a0c702867523aa997755140b/68747470733a2f2f63692e6170707665796f722e636f6d2f6170692f70726f6a656374732f7374617475732f657038787732326365786b74676862613f7376673d74727565)

Simple .NET Framework Wrapper for Rest calls API for the Firebase RealTime Database.

### Installation
```
 Download and build project in Visual Studio. Once build, add reference to SharpFireStarter.dll to you project
```

### Supported Frameworks
```
 .NET Framework 4.5.2
```

### Todos
 - Add project to Nuget Repo
 - Better Error Handling
 - Add Delete Data Functionality
 - Lower .Net Version Target

## Usage
### Instantiate
```csharp
//Init Database Connection
//Get your appID from the Firebase Console
//Get your apiKey from the Firebase Console
SharpFireStarter.FireBaseDB db = new SharpFireStarter.FireBaseDB(appID, apiKey);
```

### Authenticate
```csharp
//Authenticate with Firebase DB
if(db.Authenticate(authUserName, authUserPass))
{
   //Successfully authenticated
}
```

### Get Data
```csharp
//Get Data from Firebase DB
string data = await db.GetFromDB("firebase/path/to/yourdata");
if(data != null)
{
    //Data retrieved successfully
}
```

### Write Data
```csharp
//Write Data to Firebase DB
 var obj = new {
    random = "yup",
    thisToo = "Okay"
};
db.WriteToDB("firebase/path/to/yourdata", obj);
```
[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

License
----

MIT


**Free Software, Hell Yeah!**
