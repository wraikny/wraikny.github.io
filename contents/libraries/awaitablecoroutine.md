---
layout: post
title: AwaitableCoroutine
published: 2021-5-8
tags: library,csharp,awaitable,coroutine
---

AwaitableCoroutine は、async/await 構文を使用可能にしたコルーチンを提供する C# 向けライブラリです。 内部的にはTask-Like、Awaitable パターン、AsyncMethodBuilder が使われています。

リポジトリ

- [AwaitableCoroutine - GitHub](https://github.com/wraikny/AwaitableCoroutine)

<!--more-->

- [AwaitableCoroutine - NuGet Gallery](https://www.nuget.org/packages/AwaitableCoroutine/)
- [AwaitableCoroutine.FSharp - NuGet Gallery](https://www.nuget.org/packages/AwaitableCoroutine.FSharp/)
- [AwaitableCoroutine.Altseed2 - NuGet Gallery](https://www.nuget.org/packages/AwaitableCoroutine.Altseed2/)

### Example

```csharp
using System;
using AwaitableCoroutine;

var runner = new CoroutineRunner();

int count = 0;

var coroutine = runner.Create(async () => {
    Console.WriteLine("Started!");

    for (var i = 0; i < 10; i++)
    {
        count++;
        await Coroutine.Yield();
    }
}).OnCompleted(() => Console.WriteLine("Finished!"));

while (true)
{
    runner.Update();
    if (coroutine.IsCompleted) break;

    Console.WriteLine($"{count}");
}
```
