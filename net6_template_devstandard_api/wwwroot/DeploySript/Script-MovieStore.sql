USE [MovieStore]
GO
/****** Object:  Table [dbo].[Movie]    Script Date: 07/03/2022 10:54:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movie](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Url] [nvarchar](3000) NULL,
 CONSTRAINT [PK_MasterMovie] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Movie] ON 

INSERT [dbo].[Movie] ([Id], [Title], [Url]) VALUES (1, N'Spider man no way home', N'https://terrigen-cdn-dev.marvel.com/content/prod/1x/snh_online_6072x9000_posed_01.jpg')
INSERT [dbo].[Movie] ([Id], [Title], [Url]) VALUES (2, N'SHANG SHI', N'https://lumiere-a.akamaihd.net/v1/images/shang-chi-poster_660fcb9f.jpeg')
SET IDENTITY_INSERT [dbo].[Movie] OFF
