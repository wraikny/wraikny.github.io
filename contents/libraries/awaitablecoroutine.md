---
layout: post
title: AwaitableCoroutine
published: 2021-5-8
tags: library,csharp,awaitable,coroutine
---

AwaitableCoroutine は、async/await 構文を使用可能にしたコルーチンを提供する C# 向けライブラリです。 内部的にはTask-Like、Awaitable パターン、AsyncMethodBuilder が使われています。

- [AwaitableCoroutine - GitHub](https://github.com/wraikny/AwaitableCoroutine)
- [AwaitableCoroutine - NuGet Gallery](https://www.nuget.org/packages/AwaitableCoroutine/)

<!--more-->

### Example

```csharp
public static class Program
{
    private static int s_count;

    private static async AwaitableCoroutine CreateCoroutine()
    {
        for (var i = 0; i < 10; i++)
        {
            s_count++;
            await AwaitableCoroutine.Yield();
        }
    }

    public static void Main(string[] args)
    {
        var runner = new CoroutineRunner();

        var coroutine = runner.AddCoroutine(CreateCoroutine);

        Console.WriteLine("Started!");

        while (!coroutine.IsCompleted)
        {
            Console.WriteLine($"{s_count}");
            runner.Update();
        }

        Console.WriteLine("Finished!");
    }
}
```
