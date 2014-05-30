# SQL Manager 2007 for MySQL 4.2.1.1
# ---------------------------------------
# Host     : 192.168.1.2
# Port     : 3306
# Database : MyData


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

SET FOREIGN_KEY_CHECKS=0;

CREATE DATABASE `MyData`
    CHARACTER SET 'utf8'
    COLLATE 'utf8_general_ci';

USE `MyData`;

#
# Structure for the `rgp_authoritydir` table : 
#

CREATE TABLE `rgp_authoritydir` (
  `AuthorityID` int(11) NOT NULL AUTO_INCREMENT,
  `AuthorityName` varchar(30) NOT NULL,
  `AuthorityTag` varchar(50) NOT NULL,
  `AuthorityDescription` varchar(50) DEFAULT NULL,
  `AuthorityOrder` int(11) NOT NULL DEFAULT '0',
  KEY `AuthorityID` (`AuthorityID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

#
# Structure for the `rgp_configuration` table : 
#

CREATE TABLE `rgp_configuration` (
  `ItemID` int(11) NOT NULL AUTO_INCREMENT,
  `ItemName` varchar(50) NOT NULL,
  `ItemValue` text,
  `ItemDescription` text,
  KEY `ItemID` (`ItemID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

#
# Structure for the `rgp_groups` table : 
#

CREATE TABLE `rgp_groups` (
  `GroupID` int(11) NOT NULL AUTO_INCREMENT,
  `GroupName` varchar(30) NOT NULL,
  `GroupOrder` int(11) NOT NULL DEFAULT '0',
  `GroupDescription` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`GroupID`),
  KEY `GroupID` (`GroupID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

#
# Structure for the `rgp_moduleauthoritylist` table : 
#

CREATE TABLE `rgp_moduleauthoritylist` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ModuleID` int(11) NOT NULL,
  `AuthorityTag` varchar(50) NOT NULL,
  KEY `ID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8;

#
# Structure for the `rgp_modules` table : 
#

CREATE TABLE `rgp_modules` (
  `ModuleID` int(11) NOT NULL AUTO_INCREMENT,
  `ModuleTypeID` int(11) NOT NULL,
  `ModuleName` varchar(30) NOT NULL,
  `ModuleTag` varchar(50) NOT NULL,
  `ModuleURL` text,
  `ModuleDisabled` bit(1) NOT NULL DEFAULT '',
  `ModuleOrder` int(11) NOT NULL DEFAULT '0',
  `ModuleDescription` varchar(50) DEFAULT NULL,
  `IsMenu` bit(1) NOT NULL DEFAULT '',
  KEY `ModuleID` (`ModuleID`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

#
# Structure for the `rgp_moduletype` table : 
#

CREATE TABLE `rgp_moduletype` (
  `ModuleTypeID` int(11) NOT NULL AUTO_INCREMENT,
  `ModuleTypeName` varchar(30) NOT NULL,
  `ModuleTypeOrder` int(11) NOT NULL DEFAULT '0',
  `ModuleTypeDescription` varchar(50) DEFAULT NULL,
  KEY `ModuleTypeID` (`ModuleTypeID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

#
# Structure for the `rgp_roleauthoritylist` table : 
#

CREATE TABLE `rgp_roleauthoritylist` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL DEFAULT '0',
  `RoleID` int(11) NOT NULL DEFAULT '0',
  `ModuleID` int(11) NOT NULL,
  `AuthorityTag` varchar(50) NOT NULL,
  `Flag` bit(1) NOT NULL DEFAULT '',
  KEY `ID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=86 DEFAULT CHARSET=utf8;

#
# Structure for the `rgp_roles` table : 
#

CREATE TABLE `rgp_roles` (
  `RoleID` int(11) NOT NULL AUTO_INCREMENT,
  `RoleGroupID` int(11) NOT NULL,
  `RoleName` varchar(30) NOT NULL,
  `RoleDescription` varchar(50) DEFAULT NULL,
  KEY `RoleID` (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

#
# Structure for the `users` table : 
#

CREATE TABLE `users` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(128) NOT NULL,
  `Password` varchar(128) NOT NULL,
  `Question` varchar(100) DEFAULT NULL,
  `Answer` varchar(100) DEFAULT NULL,
  `RoleID` int(11) NOT NULL DEFAULT '0',
  `UserGroup` int(11) NOT NULL DEFAULT '0',
  `CreateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastLoginTime` datetime DEFAULT NULL,
  `Status` int(11) NOT NULL DEFAULT '1',
  `IsOnline` bit(1) NOT NULL DEFAULT '\0',
  `IsLimit` bit(1) NOT NULL DEFAULT '\0',
  KEY `UserID` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

#
# Data for the `rgp_authoritydir` table  (LIMIT 0,500)
#

INSERT INTO `rgp_authoritydir` (`AuthorityID`, `AuthorityName`, `AuthorityTag`, `AuthorityDescription`, `AuthorityOrder`) VALUES 
  (1,'AItem1','RGP_BROWSE','页面访问权限',1),
  (2,'AItem2','RGP_ADD','页面中的添加操作权限',2),
  (3,'AItem3','RGP_EDIT','页面中的编辑修改操作权限',3),
  (4,'AItem4','RGP_DELETE','页面中的删除操作权限',4),
  (5,'AItem5','RGP_SEARCH','数据搜索权限',5),
  (6,'AItem6','RGP_VERIFY','记录审核权限',6),
  (7,'AItem7','RGP_MOVE','移动记录的权限',7),
  (8,'AItem8','RGP_PRINT','打印文档权限',8),
  (9,'AItem9','RGP_DOWNLOAD','下载权限',9),
  (10,'AItem10','RGP_BACK','资料备份权限',10);

COMMIT;

#
# Data for the `rgp_configuration` table  (LIMIT 0,500)
#

INSERT INTO `rgp_configuration` (`ItemID`, `ItemName`, `ItemValue`, `ItemDescription`) VALUES 
  (1,'InitRoleID','3','注册时初始化权限，设置值必须大于0'),
  (2,'initGroupID','3','注册时初始化用户组，设置值必须大于0'),
  (3,'IsVerifyUser','1','注册时是否启用审核，启用则不写入用户权限和用户组信息，审核时写入,输入值只能为0或1');

COMMIT;

#
# Data for the `rgp_groups` table  (LIMIT 0,500)
#

INSERT INTO `rgp_groups` (`GroupID`, `GroupName`, `GroupOrder`, `GroupDescription`) VALUES 
  (1,'group1',1,'拥有系统最高权限'),
  (2,'group2',2,'具用所有管理权限，无系统配置权限'),
  (3,'group3',3,'无后台管理权限');

COMMIT;

#
# Data for the `rgp_moduleauthoritylist` table  (LIMIT 0,500)
#

INSERT INTO `rgp_moduleauthoritylist` (`ID`, `ModuleID`, `AuthorityTag`) VALUES 
  (1,9,'RGP_BROWSE'),
  (2,9,'RGP_ADD'),
  (3,9,'RGP_EDIT'),
  (4,9,'RGP_DELETE'),
  (5,9,'RGP_SEARCH'),
  (6,9,'RGP_VERIFY'),
  (7,9,'RGP_MOVE'),
  (8,9,'RGP_PRINT'),
  (9,9,'RGP_DOWNLOAD'),
  (10,9,'RGP_BACK'),
  (11,1,'RGP_BROWSE'),
  (12,1,'RGP_ADD'),
  (13,1,'RGP_EDIT'),
  (14,1,'RGP_DELETE'),
  (15,1,'RGP_SEARCH'),
  (16,1,'RGP_VERIFY'),
  (17,1,'RGP_MOVE'),
  (18,8,'RGP_BROWSE'),
  (19,8,'RGP_EDIT'),
  (20,7,'RGP_BROWSE'),
  (21,7,'RGP_ADD'),
  (22,7,'RGP_EDIT'),
  (23,7,'RGP_DELETE'),
  (24,2,'RGP_BROWSE'),
  (25,2,'RGP_ADD'),
  (26,2,'RGP_EDIT'),
  (27,2,'RGP_DELETE'),
  (28,4,'RGP_BROWSE'),
  (29,4,'RGP_ADD'),
  (30,4,'RGP_EDIT'),
  (31,4,'RGP_DELETE'),
  (32,5,'RGP_BROWSE'),
  (33,5,'RGP_ADD'),
  (34,5,'RGP_EDIT'),
  (35,5,'RGP_DELETE'),
  (36,3,'RGP_BROWSE'),
  (37,3,'RGP_ADD'),
  (38,3,'RGP_EDIT'),
  (39,3,'RGP_DELETE'),
  (40,6,'RGP_BROWSE'),
  (41,6,'RGP_ADD'),
  (42,6,'RGP_EDIT'),
  (43,6,'RGP_DELETE');

COMMIT;

#
# Data for the `rgp_modules` table  (LIMIT 0,500)
#

INSERT INTO `rgp_modules` (`ModuleID`, `ModuleTypeID`, `ModuleName`, `ModuleTag`, `ModuleURL`, `ModuleDisabled`, `ModuleOrder`, `ModuleDescription`, `IsMenu`) VALUES 
  (1,2,'menu0301','Mod_Member','../Modubles/Users/ListUsers.aspx',True,1,'外部注册会员管理',True),
  (2,1,'menu0102','Mod_Groups','../Modubles/sysconfig/GroupsPage.aspx',True,2,'用户分组，角色分组',True),
  (3,1,'menu0105','Mod_Roles','../Modubles/sysconfig/RolesPage.aspx',True,5,'管理角色',True),
  (4,1,'menu0103','Mod_ModuleType','../Modubles/sysconfig/ModuleTypePage.aspx',True,3,'功能模块分类',True),
  (5,1,'menu0104','Mod_Modules','../Modubles/sysconfig/ModulesPage.aspx',True,4,'模块管理',True),
  (6,1,'menu0106','Mod_RoleAuthority','../Modubles/sysconfig/RoleAuthorizedPage.aspx',True,6,'',True),
  (7,1,'menu0107','Mod_Authorized','../Modubles/sysconfig/AuthorityPage.aspx',True,1,'',True),
  (8,3,'menu0201','Mod_Config','../Modubles/sysconfig/ConfigPage.aspx',True,1,'系统管理设置',True),
  (9,4,'menu0401','Mod_Control','../Modubles/Demo/GeneralControl.aspx',True,1,'一般组件的控制',True);

COMMIT;

#
# Data for the `rgp_moduletype` table  (LIMIT 0,500)
#

INSERT INTO `rgp_moduletype` (`ModuleTypeID`, `ModuleTypeName`, `ModuleTypeOrder`, `ModuleTypeDescription`) VALUES 
  (1,'menu01',4,'样例演示'),
  (2,'menu03',2,'系统配置相关功能'),
  (3,'menu02',3,'会员,公司员工管理'),
  (4,'menu04',1,'系统权限管理');

COMMIT;

#
# Data for the `rgp_roleauthoritylist` table  (LIMIT 0,500)
#

INSERT INTO `rgp_roleauthoritylist` (`ID`, `UserID`, `RoleID`, `ModuleID`, `AuthorityTag`, `Flag`) VALUES 
  (1,0,1,9,'RGP_BROWSE',True),
  (2,0,1,9,'RGP_ADD',True),
  (3,0,1,9,'RGP_EDIT',True),
  (4,0,1,9,'RGP_DELETE',True),
  (5,0,1,9,'RGP_SEARCH',True),
  (6,0,1,9,'RGP_VERIFY',True),
  (7,0,1,9,'RGP_MOVE',True),
  (8,0,1,9,'RGP_PRINT',True),
  (9,0,1,9,'RGP_DOWNLOAD',True),
  (10,0,1,9,'RGP_BACK',True),
  (11,0,1,1,'RGP_BROWSE',True),
  (12,0,1,1,'RGP_ADD',True),
  (13,0,1,1,'RGP_EDIT',True),
  (14,0,1,1,'RGP_DELETE',True),
  (15,0,1,1,'RGP_SEARCH',True),
  (16,0,1,1,'RGP_VERIFY',True),
  (17,0,1,1,'RGP_MOVE',True),
  (18,0,1,8,'RGP_BROWSE',True),
  (19,0,1,8,'RGP_EDIT',True),
  (20,0,1,7,'RGP_BROWSE',True),
  (21,0,1,7,'RGP_ADD',True),
  (22,0,1,7,'RGP_EDIT',True),
  (23,0,1,7,'RGP_DELETE',True),
  (24,0,1,2,'RGP_BROWSE',True),
  (25,0,1,2,'RGP_ADD',True),
  (26,0,1,2,'RGP_EDIT',True),
  (27,0,1,2,'RGP_DELETE',True),
  (28,0,1,4,'RGP_BROWSE',True),
  (29,0,1,4,'RGP_ADD',True),
  (30,0,1,4,'RGP_EDIT',True),
  (31,0,1,4,'RGP_DELETE',True),
  (32,0,1,5,'RGP_BROWSE',True),
  (33,0,1,5,'RGP_ADD',True),
  (34,0,1,5,'RGP_EDIT',True),
  (35,0,1,5,'RGP_DELETE',True),
  (36,0,1,3,'RGP_BROWSE',True),
  (37,0,1,3,'RGP_ADD',True),
  (38,0,1,3,'RGP_EDIT',True),
  (39,0,1,3,'RGP_DELETE',True),
  (40,0,1,6,'RGP_BROWSE',True),
  (41,0,1,6,'RGP_ADD',True),
  (42,0,1,6,'RGP_EDIT',True),
  (43,0,1,6,'RGP_DELETE',True),
  (44,0,2,9,'RGP_BROWSE',True),
  (45,0,2,9,'RGP_ADD',True),
  (46,0,2,9,'RGP_EDIT',True),
  (47,0,2,9,'RGP_DELETE',True),
  (48,0,2,9,'RGP_SEARCH',True),
  (49,0,2,9,'RGP_VERIFY',True),
  (50,0,2,9,'RGP_MOVE',True),
  (51,0,2,9,'RGP_PRINT',True),
  (52,0,2,9,'RGP_DOWNLOAD',True),
  (53,0,2,9,'RGP_BACK',True),
  (54,0,2,1,'RGP_BROWSE',True),
  (55,0,2,1,'RGP_ADD',True),
  (56,0,2,1,'RGP_EDIT',True),
  (57,0,2,1,'RGP_DELETE',True),
  (58,0,2,1,'RGP_SEARCH',True),
  (59,0,2,1,'RGP_VERIFY',True),
  (60,0,2,1,'RGP_MOVE',True),
  (62,0,3,9,'RGP_BROWSE',True),
  (63,0,3,9,'RGP_ADD',True),
  (64,0,3,9,'RGP_EDIT',True),
  (65,0,3,9,'RGP_DELETE',True),
  (66,0,4,9,'RGP_BROWSE',True),
  (67,0,8,9,'RGP_BROWSE',True),
  (68,0,8,9,'RGP_ADD',True),
  (69,0,8,9,'RGP_EDIT',True),
  (70,0,8,9,'RGP_DELETE',True),
  (71,0,8,9,'RGP_SEARCH',True),
  (72,0,8,9,'RGP_VERIFY',True),
  (73,0,8,9,'RGP_MOVE',True),
  (74,0,8,9,'RGP_PRINT',True),
  (75,0,8,9,'RGP_DOWNLOAD',True),
  (76,0,8,9,'RGP_BACK',True),
  (77,0,8,1,'RGP_BROWSE',True),
  (78,0,8,1,'RGP_ADD',True),
  (79,0,8,1,'RGP_EDIT',True),
  (80,0,8,1,'RGP_DELETE',True),
  (81,0,8,1,'RGP_SEARCH',True),
  (82,0,8,1,'RGP_VERIFY',True),
  (83,0,8,1,'RGP_MOVE',True),
  (84,0,8,8,'RGP_BROWSE',True),
  (85,0,8,8,'RGP_EDIT',True);

COMMIT;

#
# Data for the `rgp_roles` table  (LIMIT 0,500)
#

INSERT INTO `rgp_roles` (`RoleID`, `RoleGroupID`, `RoleName`, `RoleDescription`) VALUES 
  (1,1,'role1','具有系统所有权限'),
  (2,2,'role2','用户信息管理'),
  (3,3,'role3','外部注册会员'),
  (4,3,'role4','内部用户信息'),
  (8,1,'role8','进行系统配置');

COMMIT;

#
# Data for the `users` table  (LIMIT 0,500)
#

INSERT INTO `users` (`UserID`, `UserName`, `Password`, `Question`, `Answer`, `RoleID`, `UserGroup`, `CreateTime`, `LastLoginTime`, `Status`, `IsOnline`, `IsLimit`) VALUES 
  (1,'admin','21232F297A57A5A743894A0E4A801FC3','Who','系统管理员',1,1,'2008-12-18 18:52:12',NULL,1,False,True),
  (2,'test1','098F6BCD4621D373CADE4E832627B4F6','test','test',3,3,'2008-12-18 19:29:12',NULL,1,False,False),
  (3,'test2','098F6BCD4621D373CADE4E832627B4F6','123123','1',2,2,'2008-12-18 19:29:56',NULL,1,False,False),
  (4,'test3','098F6BCD4621D373CADE4E832627B4F6','123123','1',8,1,'2008-12-18 19:30:03',NULL,1,False,False);

COMMIT;



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;