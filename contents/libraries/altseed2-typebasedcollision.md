---
layout: post
title: Altseed2.TypeBasedCollision
published: 2021-5-5
tags: library,altseed2,csharp
---

Altsees2.TypeBasedCollision は、型をキーとして衝突対象の管理を平易かつ高速に行うための Altseed2 拡張ライブラリです。

- [wraikny/Altseed2.TypeBasedCollision - GitHub](https://github.com/wraikny/Altseed2.TypeBasedCollision)
- [Altseed2.TypeBasedCollision - NuGet Gallery](https://www.nuget.org/packages/Altseed2.TypeBasedCollision/2.0.0)

<!--more-->

### Example

```csharp
using System;
using Altseed2;
using Altseed2.TypeBasedCollision;

private abstract record Key(string Label) : ICollisionMarker;
private sealed record KeyPlayer(string Label) : Key(Label);
private sealed record KeyObstacle(string Label) : Key(Label);
private sealed record KeyEnemy(string Label) : Key(Label);

public static void Main(string[] _)
{
    Engine.Initialize("Example", 800, 600);

    var playerKey = new KeyPlayer("Player");
    var player = new CollCircleNode<KeyPlayer>(playerKey, 20.0f);

    var enemyKey = new KeyEnemy("Enemy1");
    var enemy = new CollCircleNode<KeyEnemy>(enemyKey, 30.0f)
        {
            Position = new Vector2F(80f, 80f),
        };
    
    var obstacleKey = new KeyObstacle("Obstacle1");
    var obstacle = new CollCircleNode<KeyObstacle>(obstacleKey, 50.0f)
        {
            Position = new Vector2F(200f, 200f)
        };

    Engine.AddNode(player);
    Engine.AddNode(enemy);
    Engine.AddNode(obstacle);

    while (Engine.DoEvents())
    {
        player.Position = Engine.Mouse.Position;

        player.CheckCollision<KeyEnemy>(target =>
            Console.WriteLine($"Player collided Enemy({target.Label})");
        );

        player.CheckCollision<KeyObstacle>(target =>
            Console.WriteLine($"Player collided Obstacle({target.Label})");
        );

        Engine.Update();
    }

    Engine.Terminate();
}

private class CollCircleNode<T> : TransformNode
    where T : Key
{
    private readonly CollisionNode<T> _collisionNode;

    public T Key { get; private set; }

    public CollCircleNode(T key, float radius)
    {
        Key = key;

        // 描画用のノードを追加
        AddChildNode(new CircleNode { Radius = radius });

        // 当たり判定用のノードを追加
        var collider = new CircleCollider { Radius = radius };
        // ジェネリックで指定した型がキーとなる
        _collisionNode = new CollisionNode<T>(key, collider);
        AddChildNode(_collisionNode);
    }

    public void CheckCollision<TargetKey>(Action<TargetKey> onCollided)
    {
        // 特定のキーを持つCollisionNodeとのみ衝突判定の処理を行う。
        _collisionNode.CheckCollision<TargetKey>(onCollided);
    }
}
```
