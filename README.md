# SharpFireStarter &nbsp; [![Tweet](https://img.shields.io/twitter/url/http/shields.io.svg?style=social)](https://twitter.com/intent/tweet?text=Check%20out%20this%20library%20that%20connects%20C-Sharp%20Projects%20with%20Google%20Firebase!&url=https://github.com/charifield/SharpFireStarter&hashtags=firebase,c-sharp,bootstraping,library,developers) &nbsp;[![Slack](https://img.shields.io/badge/slack-chat-green.svg)](https://join.slack.com/t/evlar/shared_invite/enQtNDkwOTgzMTk1NzYwLTJmOWE2NjJmY2UwY2UzZWM0NzMzNWI2MzQ0YTYzMDAwOGM2ZWZiNWU0NWNmOTk0ZTI2YjFiNTc4NTgwYjEwM2Q)

![Price](https://img.shields.io/badge/price-free-blue.svg) &nbsp; ![Building](https://img.shields.io/badge/build-passing-brightgreen.svg) &nbsp; ![Author](https://img.shields.io/badge/author-field%20chari-orange.svg) &nbsp; ![Version](https://img.shields.io/badge/version-ALPHA-blue.svg) 

A Light Weight .NET Framework Wrapper for Rest calls API for the Firebase RealTime Database! Somebody had to do it :)

## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 
```
//Download DLL
Download the SharpFireStarter.dll file in the debug folder and reference it to your project.

//Build on your Machine
 Download the project add reference it to your project
```

## Prerequisites

```
A Visual Studio .NET Project.
A computer
```

## Supported Frameworks
```
 .NET Framework 4.5.2
```

## Todos &nbsp; ![ToDos](https://img.shields.io/badge/completion-80%25-orange.svg)
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


#### Attribution
![Pass](https://upload.wikimedia.org/wikipedia/commons/thumb/3/3c/Cc-by_new.svg/40px-Cc-by_new.svg.png)Licensees may copy, distribute, display and perform the work and make derivative works and remixes based on it only if they give the author or licensor the credits (attribution) in the manner specified by these.