USE [MetacognitiveTutor]
GO
/****** Object:  Table [dbo].[Lessons]    Script Date: 7/14/2018 10:34:31 PM ******/
CREATE TABLE [dbo].[Lessons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookTitle] [nvarchar](256) NULL,
	[BookAmazonUrl] [nvarchar](2083) NULL,
	[TheHookYouTubeVideo] [nvarchar](max) NULL,
	[TheTwoVocabularyWordsYouTubeVideo] [nvarchar](max) NULL,
	[MainIdea] [nvarchar](max) NULL,
	[SupportingIdea] [nvarchar](max) NULL,
	[StoryDetails] [nvarchar](max) NULL,
	[StoryQuestions] [nvarchar](max) NULL,
	[ImportantSentencesForWordScramble] [nvarchar](max) NULL,
	[Provider] [varchar](30) NOT NULL,
	[ProviderId] [varchar](300) NOT NULL,
	[CreateDateUtc] [datetime2](7) NOT NULL,
	[UpdateDateUtc] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Lessons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Lessons] ADD  CONSTRAINT [DF_Lessons_CreateDateUtc]  DEFAULT (sysutcdatetime()) FOR [CreateDateUtc]
GO
ALTER TABLE [dbo].[Lessons] ADD  CONSTRAINT [DF_Lessons_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Lessons]  WITH CHECK ADD  CONSTRAINT [FK_Lessons_Users] FOREIGN KEY([Provider], [ProviderId])
REFERENCES [dbo].[Users] ([Provider], [ProviderId])
GO
ALTER TABLE [dbo].[Lessons] CHECK CONSTRAINT [FK_Lessons_Users]
GO
