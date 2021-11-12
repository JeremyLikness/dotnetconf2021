# GraphQL Example

🎞️ [Video](https://youtu.be/4nqjB_z5CU0)

This example comes with a pre-made SQLite database. Simply compile, then run, and navigate to `/graphql` to open the interface. There, you can explore the schema and write queries. Here's an example to help you get started:

```graphql
query {
  recipes {
    name,
    ingredients(order: {name: ASC}) {
      name
    }
  }
}
```
