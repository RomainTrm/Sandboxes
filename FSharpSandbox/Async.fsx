// cf : https://www.youtube.com/watch?v=u2SlQ5WdL2k&list=PL-nSd-yeckKhsHa6Hr90oYjfFHGiRbPF4

type User = User of System.Guid

type Post = 
    {
        Content: string
        NumberOfView: int
    }

// Working synchronously

let user username : User option = 
    System.Guid.NewGuid()
    |> User
    |> Some

let postsOfUser (user : User option) : Post list =
    match user with
    | Some user -> 
        [
            { NumberOfView = 10; Content = "Post1" }
            { NumberOfView = 2; Content = "Post2" }
            { NumberOfView = 15; Content = "Post3" }
            { NumberOfView = 50; Content = "Post4" }
            { NumberOfView = 5; Content = "Post5" }
        ]
    | None -> []    

let top3 posts = 
    posts
    |> List.sortByDescending (fun post -> post.NumberOfView)
    |> List.take 3

let content posts = 
    posts
    |> List.map (fun post -> post.Content)

user "Romain"
|> postsOfUser
|> top3
|> content

// Working asynchronously

let asyncUser username = 
    async {
        return user username
    }

let asyncPostsOfUser user =
    async {
        return postsOfUser user
    }

let asyncBind binder asyncValue =
    async {
        let! value = asyncValue
        return! binder value
    }

let asyncMap mapper asyncValue =
    async {
        let! value = asyncValue
        return mapper value
    }

let userPostsByPiping = 
    asyncUser "Romain"
    |> asyncBind asyncPostsOfUser
    |> asyncMap top3
    |> asyncMap content
    |> Async.RunSynchronously

let userPostsByComputationExpression = 
    async {
        let! user = asyncUser "Romain"
        let! posts = asyncPostsOfUser user
        return posts
            |> top3
            |> content 
    }
    |> Async.RunSynchronously
