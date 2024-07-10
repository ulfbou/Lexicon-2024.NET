# Endpoints: Multi-tenant web api 

# Entities and Relations

A User can be a:
- Teacher
- Student

A User can:
- Teach a Course
- Enroll Courses

A Course can have:
- Announcements
- Grades
- Students
- Teachers
- Assignments
- Exams
- Quizzes
- Resources
- Discussions

A Role can be:
- Admin
- Teacher
- Student
- Parent

A Resource can be:
- File
- Link
- Video
- Image
- Document
- Presentation
- Spreadsheet

A Tenant can have:
- Users
- Courses
- Roles
- Resources
- Announcements

A User can have:
- Roles
- Courses
- Resources
- Announcements
- Grades
- a Tenant
- a Parent

A Role can have:
- Users
- Permissions
- a Tenant

## Entity models

```csharp
public class User
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public List<Role> Roles { get; set; }
	public List<Course> Courses { get; set; }
	public List<Resource> Resources { get; set; }
	public List<Announcement> Announcements { get; set; }
	public List<Grade> Grades { get; set; }
	public Tenant Tenant { get; set; }
	public User Parent { get; set; }
}

public class Role
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; }
	public List<User> Users { get; set; }
	public List<Permission> Permissions { get; set; }
	public Tenant Tenant { get; set; }
}

public class Course
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; }
	public List<Announcement> Announcements { get; set; }
	public List<Grade> Grades { get; set; }
	public List<User> Students { get; set; }
	public List<User> Teachers { get; set; }
	public List<Assignment> Assignments { get; set; }
	public List<Exam> Exams { get; set; }
	public List<Quiz> Quizzes { get; set; }
	public List<Resource> Resources { get; set; }
	public List<Discussion> Discussions { get; set; }
	public Tenant Tenant { get; set; }
}

public class Role
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; }
	public List<User> Users { get; set; }
	public List<Permission> Permissions { get; set; }
	public Tenant Tenant { get; set; }
}

public class Resource
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; }
	public string Type { get; set; }
	public string Url { get; set; }
	public Tenant Tenant { get; set; }
}

public class Announcement
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Title { get; set; }
	public string Content { get; set; }
	public DateTime Date { get; set; } = DateTime.Now;
	public Course Course { get; set; }
}

public class Grade
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; }
	public double Value { get; set; }
	public Course Course { get; set; }
}


Understanding the Requirements
Before we dive into the code, let's clarify the expected behavior of each endpoint:

Authentication
/auth/login POST: Authenticates a user and returns a token.
/auth/register POST: Registers a new user.
Courses
/courses GET: Retrieves a list of all courses.
/courses POST: Creates a new course.
/courses/{courseId} DELETE: Deletes a course by ID.
/courses/{courseId} PUT: Updates a course by ID.
/courses/{courseId}/announcement POST: Creates a new announcement for a course.
/courses/{courseId}/grade POST: Creates a new grade for a course.
Roles
/roles GET: Retrieves a list of all roles.
/roles/assign POST: Assigns a role to a user.
/roles/remove POST: Removes a role from a user.
/roles/user GET: Retrieves a list of roles for a user.
Other
/tenants GET: Retrieves a list of all tenants.
/users GET: Retrieves a list of all users.
/users/update PUT: Updates a user.
Assumptions
We'll use in-memory data structures for simplicity.
The data models for users, courses, roles, announcements, grades, and tenants are basic C# classes.
The response format is JSON.

The endpoints are implemented with Azure Functions
The code for each endpoint is contained in a separate Azure Function.
The endpoints are secured with JWT authentication.
The endpoints are organized into separate classes for each resource (e.g., CoursesFunction, RolesFunction).
The endpoints are tested using Postman.

## Setting Up the Project

Create a new Azure Functions project in Visual Studio.
Add a new class library project to the solution for the data models.
Add a reference to the class library project in the Azure Functions project.
Add the required NuGet packages for JWT authentication and JSON serialization.
Create a new class for the JWT token handler.
Create a new class for the data repository.
Create a new class for each resource (e.g., CoursesFunction, RolesFunction).
Implement the required endpoints in each resource class.
Test the endpoints using Postman.

## Project structure 

The project structure is as follows:

```
FunctionApp
│
├── DataModels
│   ├── Course.cs
│   ├── Role.cs
	├── User.cs
	├── 
	│
	├── Functions
	│   ├── CoursesFunction.cs
	│   ├── RolesFunction.cs
	│   ├── UsersFunction.cs
	│
	├── Services
	│   ├── JwtTokenHandler.cs
	│   ├── UserRepository.cs
	│
	├── Extensions.cs
	├── Program.cs
```





## Interfaces 



