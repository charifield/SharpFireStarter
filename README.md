# SharpFireStarter

![Pass](https://camo.githubusercontent.com/5b9a317a67c1a001a0c702867523aa997755140b/68747470733a2f2f63692e6170707665796f722e636f6d2f6170692f70726f6a656374732f7374617475732f657038787732326365786b74676862613f7376673d74727565)


Simple .NET Framework Wrapper for Rest calls API for the Firebase RealTime Database.

### Installation
```
//Download DLL
Download the SharpFireStarter.dll file in the debug folder and reference it to your project.

//Build on your Machine
 Download the project add reference it to your project
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


#### Creative Commons Lisence
![Pass](https://upload.wikimedia.org/wikipedia/commons/thumb/3/3c/Cc-by_new.svg/40px-Cc-by_new.svg.png)Licensees may copy, distribute, display and perform the work and make derivative works and remixes based on it only if they give the author or licensor the credits (attribution) in the manner specified by these.


**Free Software, Hell Yeah!**
