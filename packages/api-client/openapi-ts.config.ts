import { defineConfig } from "@hey-api/openapi-ts";

export default defineConfig({
  input: "../../openapi.json",
  output: {
    path: "./src/generated",
  },
  plugins: [
    "@hey-api/client-fetch",
    "@hey-api/schemas",
    "@hey-api/sdk",
    "@hey-api/typescript",
    {
      name: "@hey-api/transformers",
      dates: true, // auto convert to ISO strings
    },
  ],
});
