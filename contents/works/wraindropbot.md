---
layout: post
title: Wraindrop Bot
published: 2022-1-12
tags: work,discord,bot
thumbnail: /images/works/wraindrop-banner.png
---

よくあるDiscordの読み上げbotです。
WindowsとRaspberry Pi (Raspbian)で動作します。

<!--more-->

機能

* テキスト読み上げ
* ユーザ読み上げ名変更
* 単語置き換え


技術構成

* 言語: F# (.NET6)
* Discord Framework: DSharpPlus
* 音声: AquesTalk Pi, ffmpeg
* データベース: SQLite3, Dapper

スタンドアロンバイナリを自宅のラズパイに置いて動かしています。

リポジトリはこちら

[wraikny/WraindropBot - GitHub](https://github.com/wraikny/WraindropBot)
