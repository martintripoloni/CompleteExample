USE [CompleteExample]
GO

/****** Object:  Table [dbo].[EnrollmentHistory]    Script Date: 4/19/2022 11:27:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EnrollmentHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnrollmentId] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[Grade] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EnrollmentHistory]  WITH CHECK ADD  CONSTRAINT [FK_EnrollmentHistory_Courses] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([CourseId])
GO

ALTER TABLE [dbo].[EnrollmentHistory] CHECK CONSTRAINT [FK_EnrollmentHistory_Courses]
GO

ALTER TABLE [dbo].[EnrollmentHistory]  WITH CHECK ADD  CONSTRAINT [FK_EnrollmentHistory_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([StudentId])
GO

ALTER TABLE [dbo].[EnrollmentHistory] CHECK CONSTRAINT [FK_EnrollmentHistory_Students]
GO

ALTER TABLE [dbo].[EnrollmentHistory]  WITH CHECK ADD  CONSTRAINT [FK_EnrollmentHistory_Enrollment] FOREIGN KEY([EnrollmentId])
REFERENCES [dbo].[Enrollment] ([EnrollmentId])
GO

ALTER TABLE [dbo].[EnrollmentHistory] CHECK CONSTRAINT [FK_EnrollmentHistory_Enrollment]
GO

USE [CompleteExample]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[CreateEnrollmentHistoryTrigger]
                                            ON [dbo].[Enrollment]
                                            AFTER UPDATE
                                        AS
										IF (UPDATE(Grade)) 
                                        BEGIN
                                         SET NOCOUNT ON;                        
                                            INSERT INTO [dbo].[EnrollmentHistory]
                                            SELECT * FROM DELETED
                                        END

