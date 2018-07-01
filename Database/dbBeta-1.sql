
-- Type package declaration
create or replace package PDTypes  
as
    TYPE ref_cursor IS REF CURSOR;
end;

-- Integrity package declaration
create or replace package IntegrityPackage AS
 procedure InitNestLevel;
 function GetNestLevel return number;
 procedure NextNestLevel;
 procedure PreviousNestLevel;
 end IntegrityPackage;
/

-- Integrity package definition
create or replace package body IntegrityPackage AS
 NestLevel number;

-- Procedure to initialize the trigger nest level
 procedure InitNestLevel is
 begin
 NestLevel := 0;
 end;


-- Function to return the trigger nest level
 function GetNestLevel return number is
 begin
 if NestLevel is null then
     NestLevel := 0;
 end if;
 return(NestLevel);
 end;

-- Procedure to increase the trigger nest level
 procedure NextNestLevel is
 begin
 if NestLevel is null then
     NestLevel := 0;
 end if;
 NestLevel := NestLevel + 1;
 end;

-- Procedure to decrease the trigger nest level
 procedure PreviousNestLevel is
 begin
 NestLevel := NestLevel - 1;
 end;

 end IntegrityPackage;
/


drop trigger "CompoundDeleteTrigger_commentr"
/

drop trigger "CompoundInsertTrigger_commentr"
/

drop trigger "CompoundUpdateTrigger_commentr"
/

alter table "answer"
   drop constraint FK_ANSWER_REFERENCE_QUESTION
/

alter table "comment"
   drop constraint FK_COMMENT_REFERENCE_USER
/

alter table "comment"
   drop constraint FK_COMMENT_REFERENCE_SCENICSP
/

alter table "commentLikeMes"
   drop constraint FK_COMMENTL_REFERENCE_MESSAGE
/

alter table "commentReply"
   drop constraint FK_COMMENTR_REFERENCE_COMMENT1
/

alter table "commentReply"
   drop constraint FK_COMMENTR_REFERENCE_COMMENT2
/

alter table "commentReplyMes"
   drop constraint FK_COMMENTR_REFERENCE_MESSAGE
/

alter table "message"
   drop constraint FK_MESSAGE_REFERENCE_USER2
/

alter table "message"
   drop constraint FK_MESSAGE_REFERENCE_USER1
/

alter table "note"
   drop constraint FK_NOTE_REFERENCE_USER
/

alter table "note"
   drop constraint FK_NOTE_REFERENCE_SCENICSP
/

alter table "noteLikeMes"
   drop constraint FK_NOTELIKE_REFERENCE_MESSAGE
/

alter table "question"
   drop constraint FK_QUESTION_REFERENCE_SCENICSP
/

alter table "scenicImage"
   drop constraint FK_SCENICIM_REFERENCE_SCENICSP
/

alter table "scenicImage"
   drop constraint FK_SCENICIM_REFERENCE_IMAGE
/

alter table "scenicPos"
   drop constraint FK_SCENICPO_REFERENCE_SCENICSP
/

alter table "strategy"
   drop constraint FK_STRATEGY_REFERENCE_SCENICSP
/

alter table "strategyImage"
   drop constraint FK_STRATEGY_REFERENCE_STRATEGY
/

alter table "strategyImage"
   drop constraint FK_STRATEGY_REFERENCE_IMAGE
/

alter table "userInfo"
   drop constraint FK_USERINFO_REFERENCE_USER
/

alter table "viewedScenicSpot"
   drop constraint FK_VIEWEDSC_REFERENCE_USER
/

alter table "viewedScenicSpot"
   drop constraint FK_VIEWEDSC_REFERENCE_SCENICSP
/

drop table "admin" cascade constraints
/

drop table "answer" cascade constraints
/

drop table "comment" cascade constraints
/

drop table "commentLikeMes" cascade constraints
/

drop table "commentReply" cascade constraints
/

drop table "commentReplyMes" cascade constraints
/

drop table "image" cascade constraints
/

drop table "message" cascade constraints
/

drop table "note" cascade constraints
/

drop table "noteLikeMes" cascade constraints
/

drop table "question" cascade constraints
/

drop table "scenicImage" cascade constraints
/

drop table "scenicPos" cascade constraints
/

drop table "scenicSpot" cascade constraints
/

drop table "strategy" cascade constraints
/

drop table "strategyImage" cascade constraints
/

drop table "user" cascade constraints
/

drop table "userInfo" cascade constraints
/

drop table "viewedScenicSpot" cascade constraints
/

create table "admin" 
(
   "adminID"            VARCHAR2(40)         not null,
   "password"           VARCHAR2(40)         not null,
   constraint PK_ADMIN primary key ("adminID")
)
/

create table "scenicSpot" 
(
   "scenicID"           NUMBER(5,0)          not null,
   "scenicName"         VARCHAR2(64)         not null,
   "scenicIntroduction" VARCHAR2(64)         not null,
   constraint PK_SCENICSPOT primary key ("scenicID")
)
/

create table "question" 
(
   "questionID"         NUMBER(8,0)          not null,
   "scenicID"           NUMBER(5,0)          not null,
   "questionContent"    VARCHAR2(400)        not null,
   constraint PK_QUESTION primary key ("questionID"),
   constraint FK_QUESTION_REFERENCE_SCENICSP foreign key ("scenicID")
         references "scenicSpot" ("scenicID")
         on delete cascade
)
/

create table "answer" 
(
   "answerID"           NUMBER(8,0)          not null,
   "questionID"         NUMBER(8,0)          not null,
   "answerContent"      VARCHAR2(400)        not null,
   constraint PK_ANSWER primary key ("answerID"),
   constraint FK_ANSWER_REFERENCE_QUESTION foreign key ("questionID")
         references "question" ("questionID")
         on delete cascade
)
/

create table "user" 
(
   "userID"             VARCHAR2(40)         not null,
   "password"           VARCHAR2(40)         not null,
   constraint PK_USER primary key ("userID")
)
/

create table "comment" 
(
   "commentID"          NUMBER(12,0)         not null,
   "userID"             VARCHAR2(40)         not null,
   "scenicID"           NUMBER(5,0)          not null,
   "mark"               FLOAT                not null,
   "commentContent"     VARCHAR2(400)        not null,
   "commentLike"        INTEGER              default 0 not null,
   "commentTime"        TIMESTAMP            not null,
   constraint PK_COMMENT primary key ("commentID"),
   constraint FK_COMMENT_REFERENCE_USER foreign key ("userID")
         references "user" ("userID")
         on delete cascade,
   constraint FK_COMMENT_REFERENCE_SCENICSP foreign key ("scenicID")
         references "scenicSpot" ("scenicID")
         on delete cascade
)
/

create table "message" 
(
   "messageID"          NUMBER(15,0)         not null,
   "senderID"           VARCHAR2(40)         not null,
   "receiverID"         VARCHAR2(40)         not null,
   "time"               TIMESTAMP            not null,
   constraint PK_MESSAGE primary key ("messageID"),
   constraint FK_MESSAGE_REFERENCE_USER2 foreign key ("senderID")
         references "user" ("userID")
         on delete cascade,
   constraint FK_MESSAGE_REFERENCE_USER1 foreign key ("receiverID")
         references "user" ("userID")
         on delete cascade
)
/

create table "commentLikeMes" 
(
   "commentID"          NUMBER(12,0)         not null,
   "messageID"          NUMBER(15,0)         not null,
   constraint PK_COMMENTLIKEMES primary key ("messageID", "commentID"),
   constraint FK_COMMENTL_REFERENCE_MESSAGE foreign key ("messageID")
         references "message" ("messageID")
         on delete cascade
)
/

create table "commentReply" 
(
   "commentID"          NUMBER(12,0)         not null,
   "replyToID"          NUMBER(12,0)         not null,
   constraint PK_COMMENTREPLY primary key ("commentID", "replyToID"),
   constraint FK_COMMENTR_REFERENCE_COMMENT1 foreign key ("commentID")
         references "comment" ("commentID")
         on delete cascade,
   constraint FK_COMMENTR_REFERENCE_COMMENT2 foreign key ("replyToID")
         references "comment" ("commentID")
         on delete cascade
)
/

create table "commentReplyMes" 
(
   "commentID"          NUMBER(12,0)         not null,
   "messageID"          NUMBER(15,0)         not null,
   constraint PK_COMMENTREPLYMES primary key ("commentID", "messageID"),
   constraint FK_COMMENTR_REFERENCE_MESSAGE foreign key ("messageID")
         references "message" ("messageID")
         on delete cascade
)
/

create table "image" 
(
   "imageID"            NUMBER(6,0)          not null,
   "imageAddress"       VARCHAR2(200)        not null,
   constraint PK_IMAGE primary key ("imageID")
)
/

create table "note" 
(
   "noteID"             NUMBER(10,0)         not null,
   "userID"             VARCHAR2(40)         not null,
   "scenicID"           NUMBER(5,0)          not null,
   "title"              VARCHAR2(80)         not null,
   "noteContent"        CLOB                 not null,
   "noteLike"           INTEGER              default 0 not null,
   "noteTime"           TIMESTAMP            not null,
   constraint PK_NOTE primary key ("noteID"),
   constraint FK_NOTE_REFERENCE_USER foreign key ("userID")
         references "user" ("userID")
         on delete cascade,
   constraint FK_NOTE_REFERENCE_SCENICSP foreign key ("scenicID")
         references "scenicSpot" ("scenicID")
         on delete cascade
)
/

create table "noteLikeMes" 
(
   "noteID"             NUMBER(10,0)         not null,
   "messageID"          NUMBER(15,0)         not null,
   constraint PK_NOTELIKEMES primary key ("noteID", "messageID"),
   constraint FK_NOTELIKE_REFERENCE_MESSAGE foreign key ("messageID")
         references "message" ("messageID")
         on delete cascade
)
/

create table "scenicImage" 
(
   "scenicID"           NUMBER(5,0)          not null,
   "imageID"            NUMBER(6,0)          not null,
   constraint PK_SCENICIMAGE primary key ("scenicID", "imageID"),
   constraint FK_SCENICIM_REFERENCE_SCENICSP foreign key ("scenicID")
         references "scenicSpot" ("scenicID")
         on delete cascade,
   constraint FK_SCENICIM_REFERENCE_IMAGE foreign key ("imageID")
         references "image" ("imageID")
)
/

create table "scenicPos" 
(
   "scenicID"           NUMBER(5,0)          not null,
   "longitue"           FLOAT                not null,
   "latitude"           FLOAT                not null,
   "address"            VARCHAR2(200)        not null,
   constraint PK_SCENICPOS primary key ("scenicID"),
   constraint FK_SCENICPO_REFERENCE_SCENICSP foreign key ("scenicID")
         references "scenicSpot" ("scenicID")
         on delete cascade
)
/

create table "strategy" 
(
   "strategyID"         NUMBER(5,0)          not null,
   "scenicID"           NUMBER(5,0)          not null,
   "title"              VARCHAR2(80)         not null,
   "content"            CLOB                 not null,
   constraint PK_STRATEGY primary key ("strategyID"),
   constraint FK_STRATEGY_REFERENCE_SCENICSP foreign key ("scenicID")
         references "scenicSpot" ("scenicID")
         on delete cascade
)
/

create table "strategyImage" 
(
   "strategyID"         NUMBER(5,0)          not null,
   "imageID"            NUMBER(6,0)          not null,
   constraint PK_STRATEGYIMAGE primary key ("strategyID", "imageID"),
   constraint FK_STRATEGY_REFERENCE_STRATEGY foreign key ("strategyID")
         references "strategy" ("strategyID")
         on delete cascade,
   constraint FK_STRATEGY_REFERENCE_IMAGE foreign key ("imageID")
         references "image" ("imageID")
)
/

create table "userInfo" 
(
   "userID"             VARCHAR2(40)         not null,
   "nickname"           VARCHAR2(40)         not null,
   "gender"             VARCHAR2(6),
   "headPortrait"       VARCHAR2(200),
   "introduction"       VARCHAR2(200),
   "phoneNumber"        NUMBER(11,0),
   constraint PK_USERINFO primary key ("userID"),
   constraint FK_USERINFO_REFERENCE_USER foreign key ("userID")
         references "user" ("userID")
         on delete cascade
)
/

create table "viewedScenicSpot" 
(
   "userID"             VARCHAR2(40)         not null,
   "scenicID"           NUMBER(5,0)          not null,
   constraint PK_VIEWEDSCENICSPOT primary key ("userID", "scenicID"),
   constraint FK_VIEWEDSC_REFERENCE_USER foreign key ("userID")
         references "user" ("userID")
         on delete cascade,
   constraint FK_VIEWEDSC_REFERENCE_SCENICSP foreign key ("scenicID")
         references "scenicSpot" ("scenicID")
         on delete cascade
)
/

