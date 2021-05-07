---
layout: post
title: Affogato
published: 2019-10-14
tags: library,fsharp
---

Affogato は F# でのゲームプログラミングを目的とした、SRTP によるベクトル型と、いくつかのアルゴリズムのためのライブラリです。

- [Affogato - GitHub](https://github.com/wraikny/Affogato)
- [Affogato - NuGet Gallery](https://www.nuget.org/packages/Affogato/)

Example

<div style='margin: 0px; padding: 0px; border: 1px solid #ececec; font-family: Monaco, Menlo, Consolas, monospace;'><style type='text/css'>.fs-str {color: #d14;} .fs-key {color: blue;} .fs-com {color: green; font-style: italic;}</style><table><tr><td style='padding: 5px; vertical-align: top; background-color: #ececec; color: rgb(160, 160, 160); font-size: 15px;'><span>1</span><br /><span>2</span><br /><span>3</span><br /><span>4</span><br /><span>5</span><br /><span>6</span><br /><span>7</span></td><td style='font-size: 15px; vertical-align: top; padding: 5px;'><pre style='margin: 0px; border: none; padding: 0; white-space: pre; font-size: 15px; background-color: white; font-family: Monaco, Menlo, Consolas, monospace;'><span class='fs-key'>open </span>Affogato
<span class='fs-key'>let </span>v1 = Vector2.init 1 0 <span class='fs-com'>// int Vector2</span>
<span class='fs-key'>let </span>v2 = Vector2.init 2.1f 3.4f <span class='fs-com'>// float32 Vector2</span>
<span class='fs-key'>let </span>v3 = v1 |&gt; Vector.map float32 <span class='fs-com'>// float32 Vector2</span>
<span class='fs-key'>let </span>v4 = v2 / v3 .* 2.0f * v2.yy + v3.xx <span class='fs-com'>// float32 Vector2</span>
<span class='fs-key'>let </span>r1 = Rectangle.init v3 v4 <span class='fs-com'>// float32 Vector2 Rectangle</span>
<span class='fs-key'>let </span>k = Vector.dot v2 v3 <span class='fs-com'>// float32</span></pre></td></tr></table><div style='font-weight: bold; padding: 10px;'>Created with <a href='http://fslight.apphb.com/' target='_blank'>FsLight</a></div></div>
