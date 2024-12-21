import { isEqual } from "lodash";
import React from "react";

const reducer = <TState>(
  prevState: TState,
  stateUpdate: Partial<TState> | ((prevState: TState) => Partial<TState>)
): TState => {
  const newPartState: Partial<TState> =
    typeof stateUpdate === "function" ? stateUpdate(prevState) : stateUpdate;

  if (newPartState == null) {
    return null;
  }

  let stateChanged = false;

  if (prevState == null) {
    stateChanged = newPartState != null;
  } else {
    const objectKeys = Object.keys(newPartState) as Array<keyof TState>;

    stateChanged = objectKeys.some((key) => {
      return !isEqual(prevState[key], newPartState[key]);
    });

    if (stateChanged) {
      return {
        ...prevState,
        ...newPartState,
      };
    }
  }
  return prevState;
};

type UserReducerResult<T> = {
  state: T;
  onUpdate: (state: Partial<T>) => void;
};

export const useGenericReducer = <T>(initialState: T): UserReducerResult<T> => {
  const [state, dispatch] = React.useReducer<React.Reducer<T, Partial<T>>>(
    (prevState: T, update: Partial<T> | ((prev: T) => Partial<T>)) =>
      reducer(prevState, update),
    initialState
  );

  return {
    state: state,
    onUpdate: dispatch,
  };
};
