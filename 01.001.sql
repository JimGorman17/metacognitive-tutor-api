USE [MetacognitiveTutor]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/4/2018 7:03:39 AM ******/
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Token] [varchar](400) NULL,
	[Email] [nvarchar](320) NULL,
	[Provider] [varchar](30) NOT NULL,
	[ProviderId] [varchar](300) NOT NULL,
	[ProviderPic] [varchar](300) NULL,
	[CreateDateUtc] [datetime2](7) NOT NULL,
	[UpdateDateUtc] [datetime2](7) NULL,
	[IsTeacher] [bit] NOT NULL,
	[IsStudent] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_Users] UNIQUE NONCLUSTERED 
(
	[Provider] ASC,
	[ProviderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreateDateUtc]  DEFAULT (sysutcdatetime()) FOR [CreateDateUtc]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsTeacher]  DEFAULT ((0)) FOR [IsTeacher]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsStudent]  DEFAULT ((0)) FOR [IsStudent]
GO

CREATE TABLE [dbo].[ErrorLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Application] [nvarchar](100) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[StackTrace] [nvarchar](max) NULL,
	[CreateDateUtc] [datetime2](7) NOT NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_ErrorLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ErrorLogs] ADD  CONSTRAINT [DF_ErrorLogs_CreateDateUtc]  DEFAULT (sysutcdatetime()) FOR [CreateDateUtc]
GO
