# 蝉翼科技基础项目架构(.NET)

蝉翼科技的所有项目基于.net平台开发，使用**PostgreSQL** 数据库。此处只列举了部分主线项目

## 1. chanyi.allservice

解决方案中包含了所有系统的**WCF服务**。基于**领域模型**架构开发。包含蝉翼产品管理相关服务，如产品版本序列号密钥管理等

## 2. chanyi.web

此项目是蝉翼的门户网站项目，基于asp.net mvc 4开发的**响应式**网站

## 3. chanyi.shepherd

此为牧羊人牧场管理系统，是核心产品项目。使用WPF **MVVM模式**基于.net framework4.0开发，兼容Window XP及更新Windows系统。

## 4. chanyi.erp

此为蝉翼内部ERP系统雏形，隐去了业务代码。

## 5. chanyi.dail

此项目是为我爱我家公司业务部门实时开发采集软件。软件分为 chrome插件 + 消息服务 + 移动App 三个部分组成。

* chrome插件负责定时扫描58同城和赶集网的最新发布房源数据，然后传送到消息服务器
* 消息服务负责消息的中转。将chrome插件采集的数据推送到app。消息通讯基于 **signalR** 技术
* 移动App使用Xamarin.Forms跨平台技术开发。负责接收到消息后自动拨号
