import React from "react";

export const useTemporaryState = <T>(initialState: T) => {
  const originalState = React.useRef(initialState);
  const [tempState, setTempState] = React.useState<T>(initialState);

  console.log("State", initialState);

  React.useEffect(() => {
    if (initialState) {
      setTempState(initialState);
    }
  }, [initialState]);

  const updateTempState = React.useCallback((stateUpdate: T) => {
    setTempState(stateUpdate);
  }, []);

  const resetTempState = React.useCallback(() => {
    setTempState(originalState.current);
  }, [originalState]);

  return {
    temporaryState: tempState,
    updateTempState,
    resetTempState,
  };
};
