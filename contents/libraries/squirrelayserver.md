---
layout: post
title: SquirrelayServer
published: 2021-10-12
tags: library,server
---

SquirrelayServer は、リアルタイム通信のためのリレーサーバーです。 設定ファイルを記述するだけで色々な種類のゲームで利用できることを目指しています。 
特に理由はないですが、名前は動物のリス（Squirrel）と掛けています。

現在開発中です。

C#で記述していて、.NET環境で利用できます。  
SerializationのライブラリとしてMessagePack-CSharpを使用しています。  
RUDP通信のライブラリとしてLiteNetLibを使用しています。

xunitとMoqを利用した単体テストを記述していて、GitHub Actionsで自動実行しています。

リポジトリはこちら

[wraikny/SquirrelayServer - GitHub](https://github.com/wraikny/SquirrelayServer)
