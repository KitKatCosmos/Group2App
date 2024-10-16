-- Table: Users
CREATE TABLE Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT, -- Changed to INTEGER
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash VARBINARY(256) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Table: Tasks
CREATE TABLE Tasks (
    TaskID INTEGER PRIMARY KEY AUTOINCREMENT, -- Changed to INTEGER
    UserID INTEGER NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Description TEXT,
    PriorityLevel TINYINT CHECK (PriorityLevel IN (1, 2, 3)), -- 1 = Low, 2 = Medium, 3 = High
    CategoryID INTEGER,
    DueDate DATETIME,
    IsCompleted BIT DEFAULT 0,
    IsRepeating BIT DEFAULT 0,
    RepeatInterval NVARCHAR(20), -- e.g., 'Daily', 'Weekly', 'Monthly'
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Table: Categories
CREATE TABLE Categories (
    CategoryID INTEGER PRIMARY KEY AUTOINCREMENT, -- Changed to INTEGER
    UserID INTEGER NOT NULL,
    CategoryName NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Table: Comments
CREATE TABLE Comments (
    CommentID INTEGER PRIMARY KEY AUTOINCREMENT, -- Changed to INTEGER
    TaskID INTEGER NOT NULL,
    UserID INTEGER NOT NULL,
    CommentText TEXT NOT NULL,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TaskID) REFERENCES Tasks(TaskID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Table: Notifications
CREATE TABLE Notifications (
    NotificationID INTEGER PRIMARY KEY AUTOINCREMENT, -- Changed to INTEGER
    UserID INTEGER NOT NULL,
    TaskID INTEGER,
    Message NVARCHAR(255) NOT NULL,
    NotificationDate DATETIME,
    IsRead BIT DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (TaskID) REFERENCES Tasks(TaskID)
);

-- Table: TaskFilters
CREATE TABLE TaskFilters (
    FilterID INTEGER PRIMARY KEY AUTOINCREMENT, -- Changed to INTEGER
    UserID INTEGER NOT NULL,
    FilterName NVARCHAR(50) NOT NULL,
    FilterCriteria TEXT, -- JSON or a structured string to define filter criteria
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
