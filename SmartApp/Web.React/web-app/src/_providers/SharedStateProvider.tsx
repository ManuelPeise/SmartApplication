import React, { createContext, useContext } from "react";

type ContextState<T> = {
  state: T;
  setState: React.Dispatch<React.SetStateAction<T>>;
};

export function createGenericContext<T>() {
  const context = createContext<ContextState<T> | undefined>(undefined);
  const useGenericContext = () => {
    const ctx = useContext(context);
    if (!ctx)
      throw new Error("useGenericContext must be used within a Provider");
    return ctx;
  };
  return [useGenericContext, context.Provider] as const;
}
