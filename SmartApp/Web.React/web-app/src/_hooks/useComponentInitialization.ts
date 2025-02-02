import React from "react";

export const useComponentInitialization = <TProps>(
  token: string,
  callback: (token: string) => Promise<TProps>
) => {
  const [data, setData] = React.useState<TProps | null>(null);
  const [isLoading, setIsLoading] = React.useState(true);

  React.useEffect(() => {
    let isInitialized = true;

    const initAsync = async () => {
      setIsLoading(true);

      try {
        const initializationResult = await callback(token);

        if (isInitialized) {
          setData(initializationResult);
        }
      } catch (err) {
        if (isInitialized) {
          throw new Error("Could not initialize component.");
        }
      } finally {
        if (isInitialized) {
          setIsLoading(false);
        }
      }
    };

    initAsync();

    return () => {
      isInitialized = false;
    };
  }, [token, callback]);

  return { isLoading, initProps: data, isInitialized: data != null };
};
