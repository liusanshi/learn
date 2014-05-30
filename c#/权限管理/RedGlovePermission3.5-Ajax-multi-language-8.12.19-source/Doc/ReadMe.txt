/*******************************************************************
/*
/* 项目名  : RedGlovePermission Preview（基于B/S的权限管理系统）
/* 作  者  : Nick.Yan
/*     QQ  : 13458753
/* E-mail  : nick.yanchen@gmail.com
/* 开源网址:http://www.codeplex.com/RedGlovePermission/
/* 技术服务:http://www.cnblog.com/nick4
/*
/*******************************************************************


　　本系统是一个小型权限管理系统，不要以大系统框架的模式来看这个框架，您在使用之前，应该先了解系统框架，是否适用您的设计需求，这个框架可以应用到中小系统中，我想是没有多大问题，本系统在权限设计方面，只能给您一个参考，本系统现在实现了对系统中的功能模块控制，开发功能模块进可以将系统权限加进去，再将相应的权限授权给角色，再将角色指派给用户。

    版本更新(Ver 8.12.20)
　　● 改换成工厂模式 
　　● 支持MySQL数据 
　　● 多语言 
　　● 多皮肤 
　　● 单点登录 
　　● 将模块权限列表加宽分成两列,方便操作更多的权限 
　　● 默认权限加到10种

1.开发环境

    操作系统：window 2000/XP/Vista/2008
    开发语言：C#,基于.net3.5
    数 据 库：Sqlserver 2000/2005/2008
    开发工具：VS2008/动软.net代码生成器

2.配置
　　(1)建立数据库
　　　　在Doc目录中有数据库脚本文件
　　(2)数据库配置
　　　　<!--SQL Server-->
        <!--<add key="DataDAL" value="RedGlovePermission.SQLServerDAL" />-->
        <!--<add key="SQLString" value="Database=MyData;Data Source=.;User Id=sa;Password=sa;"/>-->
        <!--My SQL-->
        <add key="DataDAL" value="RedGlovePermission.MySqlDAL" />
        <add key="SqlString" value="host=localhost;userid=root;password=root;database=MyData"/>
　　(3)启用 Asp.net state Service服务
　　　　<sessionState mode="StateServer" stateConnectionString="tcpip=127.0.0.1:42424" stateNetworkTimeout="14400" timeout="18000"/>
    (3)样式配置
　　　　<StyleList>
　　　　  <add key="Default" value="Default" />
　　　　  <add key="White"   value="White" />
　　　　</StyleList>  
    (4)语言配置
　　　　<WebLanguage>    
    　　　　<add key="zh-cn" value="简体" />
    　　　　<add key="zh-tw" value="繁體" />
    　　　　<add key="en-us" value="English" />    
　　　　</WebLanguage>

3.系统操作流程

　　首先，在系统使用前，必须先配置好权限，流程如下：
 
　　(1)建立权限列表，加添需要权限，初始权限(浏览/新增/编辑/删除/搜索/审核/移动/打印/下载/备份)
　　(2)建立用户组管理,为了方便将用户分类
　　(3)建立模块分类,将功能模块分类
　　(4)建立模块管理,添加子模块，详细功能权限设置
　　(5)建立角色管理
　　(6)建立角色授权，将模块权限权限给角色

　　测试用户
　　用户名　      密码
　　Admin        admin
　　test1         test
　　test2         test
　　test3         test

4.开发计划 

本系统所以有数据库数据操都写在代码中，无存储过程，支持多数据库,改为工厂模式先支持Access,sqlser,mysql,后支持Oracle 
将模块权限列表加宽分成两列,方便操作更多的权限 
多语言版 
多皮肤 
夸域权限管理 
单独设定权限 
多角色 
多登录模式（域/form/单点） 
用户配置库
    用户ID  配置名　配置值　数据类型　是否启用验证　显示类型（lab,list,chkbox) 
    近请关注。。。

5.版权申明
　　本系统完全开源，免费使用，如果你要使用，希望您能保留版权信息，本系统会不断完善更新有什么问题给发送邮件，如果您有好见意或意见，但说无访，希望这个系统真能为您帮上点忙，那就是我最开心的事了，也希望更多的朋友加入进来，先申明，没薪水的啊，呵呵，目的在于分享自己成功

6.常见问题
　(1)２.０版在VS2005中打开的解决办法

   打开用记事本工程文件*.csproj,作以下修改即可。
   第一步：将<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003" ToolsVersion="3.5">改成
   　　　　<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">

　第二步：再在文件中找到
   　　　　<AssemblyName>RedGlovePermission.Web</AssemblyName>
   　　　　　　<FileUpgradeFlags>
   　　　　　　</FileUpgradeFlags>
   　　　　　　<OldToolsVersion>2.0</OldToolsVersion>
   　　　　　　<UpgradeBackupLocation>
   　　　　　　</UpgradeBackupLocation>
   　　　　</PropertyGroup>
　　　　将<AssemblyName>RedGlovePermission.Web</AssemblyName>与</PropertyGroup>之前设置删除,修改之后即
　　　　　　<AssemblyName>RedGlovePermission.Web</AssemblyName>
  　　　　　</PropertyGroup>

　第三步：将<Import Project="$(MSBuildExtensionsPath)\Microsoft\VisualStudio\v9.0\WebApplications\Microsoft.WebApplication.targets" Condition="" />删除，没有就不用管它了

　第四步：经过上面三步如果你还不能打开的话<Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" />替换成
　　　　　<Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" />，看起来是一样的，我在使用的时候就遇到了这个问题

　(2)数据库在导入到sql 2000中　

　现在提供2005的数据脚本，Sql2000脚本下载
