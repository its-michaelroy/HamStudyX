# **QuizIQ || HamStudyX**  
*Expanding ConsoleIQ into an Enhanced Quiz Application*

---

## **Table of Contents**

1. [Statement of Purpose](#statement-of-purpose)  
2. [Runtime Environment](#runtime-environment)  
3. [Information Needed](#information-needed)  
4. [Data to be Persisted (Database)](#data-to-be-persisted-database)  
5. [App Concerns](#app-concerns)  
6. [User Interface Outline and Functional Flow](#user-interface-outline-and-functional-flow)  
7. [Absolute Features](#absolute-features)
8. [Future Considerations](#future-considerations)
---

## **1. Statement of Purpose**

**Title**: QuizIQ  
**Purpose**:  
QuizIQ builds upon the existing ConsoleIQ application to create a robust, cross-platform quiz application that enhances user engagement and learning. With features like user profiles, progress tracking, diverse question types, and a graphical user interface using .NET MAUI, QuizIQ aims to facilitate interactive education across a wide range of topics.

---

## **2. Runtime Environment**

- **Platform**: Desktop and Mobile (Cross-platform)  
- **Operating Systems**:  
  - Windows, macOS, Linux, Android, iOS  

- **Technologies**:  
  - **Framework**: .NET 8.0  
  - **Programming Language**: C# 12.0  
  - **User Interface**: .NET MAUI (Multi-platform App UI) for cross-platform GUI development  

- **Services Needed**:  
  - **Local and Remote Database**: SQL Server, SQLite, MySQL or MariaDB for data persistence  
  - **Optional Cloud Services**:  
    - User authentication and data synchronization across devices  
    - Online leaderboards and community-shared quizzes  (FUTURE)

---

## **3. Information Needed**

### **User Data**  
- **Account Details**:  
  - Username  
  - Password (securely hashed)  
  - Email address (for account recovery and notifications)  

- **Profile Information**:  
  - Avatar or profile picture  

### **Quiz Content**  
- **Topics and Categories**  
- **Questions**:  
  - Question ID  
  - Type (e.g., Multiple Choice, True/False, Matching)  
  - Prompt  
  - Options (for applicable types)  
  - Correct Answer(s)  
  - Difficulty Level  
  - Explanations or hints  

### **User Progress Data**  
- Quiz history and scores  
- Time spent on quizzes  

---

## **4. Data to be Persisted (Database)**

### **Users Table**  
- UserID (Primary Key)  
- Username  
- PasswordHash  
- Email  
- Avatar  

### **Quizzes Table**  
- QuizID  
- Title  
- Description  
- Author (UserID if user-generated)  
- TopicID   

### **Questions Table**  
- QuestionID  
- QuizID (Foreign Key)  
- Type  
- Prompt  
- Options (stored as JSON or in a related table)  
- CorrectAnswer  
- Explanation  
- DifficultyLevel  

### **Topics Table**  
- TopicID  
- Name  
- Description  

### **UserProgress Table**  
- ProgressID  
- UserID  
- QuizID  
- Score  
- DateTaken  
- TimeSpent  

### **Settings Table**  
- SettingID  
- UserID  
- Preferences (serialized settings data)  

---

## **5. App Concerns**

### **Security**  
- Implement secure authentication and authorization mechanisms.  
- Protect user credentials with robust hashing algorithms (e.g., bcrypt).  
- Prevent common vulnerabilities (e.g., SQL injection, cross-site scripting).  

### **Privacy**  
- Comply with data protection regulations (e.g., GDPR, CCPA).  
- Provide clear privacy policies and obtain user consent for data collection.  
- Allow users to control their data (e.g., download, delete account).  

### **Data Availability**  
- Ensure offline access to quizzes and user data.  
- Implement data synchronization strategies for when connectivity is available.  

### **Accessibility**  
- Design the UI with accessibility in mind (keyboard navigation, screen reader support).  
- Offer customization options for different user needs.  

### **Scalability**  
- Design the system to accommodate increasing amounts of data and users.  
- Optimize performance for a smooth user experience.  

---

## **6. User Interface Outline and Functional Flow**

### **Splash Screen**  
- Application logo and loading indicator.  

### **Login/Register Screen**  
#### **Login Tab**  
- Fields: Username, Password.  
- Buttons: Login, Forgot Password.  

#### **Register Tab**  
- Fields: Username, Email, Password, Confirm Password.  
- Buttons: Register.  

### **Main Dashboard**  
- **Navigation Menu**:  
  - Home, Quizzes, Progress, Logout.   

### **Quiz Selection Screen**  
- **Filters**:  
  - Topics, Difficulty Levels, Quiz Types.  
- **Quiz List**:  
  - Quiz title, brief description.  
  - Option to select and start a quiz.  

### **Quiz Interface**  
- **Question Display**:  
  - Show question prompt and options (if applicable).  
  - Progress bar indicating quiz completion.   
- **Timer** (optional).  

### **Quiz Results Screen**  
- **Score Summary**:  
  - Total score, percentage.  
- **Detailed Feedback**:  
  - Correct answers and explanations.  
  - Option to review questions.  
 
 ---

## **7. Absolute Features**

### **Must-Haves**
- **Cross-Platform User Interface**:
  - Implement a clean, intuitive UI with .NET MAUI.
- **Login/Logout/Register**:
  - Allow users to create accounts, log in, and log out.
  - Store user credentials securely (e.g., hashed passwords).
- **Quiz Management**:
  - Display quizzes by topic or shuffled order.
  - Navigate questions with Next/Previous buttons.
- **Quiz History and Scores**:
  - Store and display user scores and quiz history.
- **Timer for Quizzes**:
  - Include an optional timer to enhance the challenge.
- **Shuffle Option**:
  - Allow users to shuffle question order.
- **Database Persistence**:
  - Use SQLite to persist user data, quiz content, and progress.
- **Exit During Gameplay**:
  - Allow users to quit or pause the quiz at any time.

---

## **8. Future Considerations**

### **Additional Features**
- **TBD**:
  - Everything else not mentioned in the Absolute Features section above.