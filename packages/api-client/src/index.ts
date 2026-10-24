import { createClient, createConfig } from "@hey-api/client-fetch";

export * from "./generated/sdk.gen.js";
export * from "./generated/types.gen.js";

export const configureApiClient = (baseUrl: string, getToken?: () => string | null) => {
  return createClient(
    createConfig({
      baseUrl,
    }),
  );
};
