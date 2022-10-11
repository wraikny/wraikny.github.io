---
layout: post
title: SquirrelayServer
published: 2021-10-12
tags: library,server,csharp
---

SquirrelayServer は、リアルタイム通信のためのリレーサーバーです。 ルーム機能もあります。設定ファイルを記述するだけで色々な種類のゲームで利用できることを目指しています。 
名前は動物のリス（Squirrel）と掛けています。

- [リアルタイムリレーサーバーSquirrelayServerの紹介 - AmusementCreators](https://www.amusement-creators.info/articles/squirrelayserver/)
- [wraikny/SquirrelayServer - GitHub](https://github.com/wraikny/SquirrelayServer)

<!--more-->

C#で記述していて、.NET環境で利用できます。  
SerializationのライブラリとしてMessagePack-CSharpを使用しています。  
RUDP通信のライブラリとしてLiteNetLibを使用しています。

xunitとMoqを利用した単体テストを記述していて、GitHub Actionsで自動実行しています。
