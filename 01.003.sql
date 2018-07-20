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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_StudentLessonAnswers]    Script Date: 7/17/2018 7:41:34 AM ******/
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