USE [MetacognitiveTutor]
GO

ALTER TABLE [dbo].[ErrorLogs]
  DROP COLUMN [UserId];

ALTER TABLE [dbo].[ErrorLogs]
	ADD	[Provider] [varchar](30) NULL,
		[ProviderId] [varchar](300) NULL;

CREATE TABLE [dbo].[StudentLessonAnswers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Provider] [varchar](30) NOT NULL,
	[ProviderId] [varchar](300) NOT NULL,
	[LessonId] [int] NOT NULL,
	[QuestionType] [varchar](20) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[Question] [nvarchar](max) NULL,
	[Answer] [nvarchar](max) NULL,
	[CreateDateUtc] [datetime2](7) NOT NULL,
	[UpdateDateUtc] [datetime2](7) NULL,
 CONSTRAINT [PK_StudentLessonAnswers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_StudentLessonAnswers] UNIQUE NONCLUSTERED 
(
	[Provider] ASC,
	[ProviderId] ASC,
	[LessonId] ASC,
	[QuestionType] ASC,
	[QuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_StudentLessonAnswers]    Script Date: 7/21/2018 11:07:41 AM ******/
CREATE NONCLUSTERED INDEX [IX_StudentLessonAnswers] ON [dbo].[StudentLessonAnswers]
(
	[Provider] ASC,
	[ProviderId] ASC,
	[LessonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudentLessonAnswers] ADD  CONSTRAINT [DF_StudentLessonAnswers_CreateDateUtc]  DEFAULT (sysutcdatetime()) FOR [CreateDateUtc]
GO
ALTER TABLE [dbo].[StudentLessonAnswers]  WITH CHECK ADD  CONSTRAINT [FK_StudentLessonAnswers_Lessons] FOREIGN KEY([LessonId])
REFERENCES [dbo].[Lessons] ([Id])
GO
ALTER TABLE [dbo].[StudentLessonAnswers] CHECK CONSTRAINT [FK_StudentLessonAnswers_Lessons]
GO
ALTER TABLE [dbo].[StudentLessonAnswers]  WITH CHECK ADD  CONSTRAINT [FK_StudentLessonAnswers_Users] FOREIGN KEY([Provider], [ProviderId])
REFERENCES [dbo].[Users] ([Provider], [ProviderId])
GO
ALTER TABLE [dbo].[StudentLessonAnswers] CHECK CONSTRAINT [FK_StudentLessonAnswers_Users]
GO


CREATE TABLE [dbo].[Grades](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LessonId] [int] NOT NULL,
	[StudentProvider] [varchar](30) NOT NULL,
	[StudentProviderId] [varchar](300) NOT NULL,
	[TeacherProvider] [varchar](30) NOT NULL,
	[TeacherProviderId] [varchar](300) NOT NULL,
	[Grade] [nvarchar](10) NULL,
	[Comments] [nvarchar](max) NULL,
	[CreateDateUtc] [datetime2](7) NOT NULL,
	[UpdateDateUtc] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Grades] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Grades]    Script Date: 7/22/2018 3:24:31 PM ******/
CREATE NONCLUSTERED INDEX [IX_Grades] ON [dbo].[Grades]
(
	[LessonId] ASC,
	[StudentProvider] ASC,
	[StudentProviderId] ASC,
	[IsDeleted] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Grades] ADD  CONSTRAINT [DF_Grades_CreateDateUtc]  DEFAULT (sysutcdatetime()) FOR [CreateDateUtc]
GO
ALTER TABLE [dbo].[Grades] ADD  CONSTRAINT [DF_Grades_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK_Grades_Lessons] FOREIGN KEY([LessonId])
REFERENCES [dbo].[Lessons] ([Id])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK_Grades_Lessons]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK_Grades_Users] FOREIGN KEY([StudentProvider], [StudentProviderId])
REFERENCES [dbo].[Users] ([Provider], [ProviderId])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK_Grades_Users]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK_Grades_Users1] FOREIGN KEY([TeacherProvider], [TeacherProviderId])
REFERENCES [dbo].[Users] ([Provider], [ProviderId])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK_Grades_Users1]
GO