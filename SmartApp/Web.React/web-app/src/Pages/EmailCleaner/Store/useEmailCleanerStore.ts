import React from "react";
import {
  EmailCleanupSettings,
  FolderMappingEntry,
} from "../Types/emailCleanupTypes";
import { isEqual } from "lodash";

const reducer = (
  prevState: EmailCleanupSettings[],
  stateUpdate:
    | Partial<EmailCleanupSettings>
    | ((prevState: EmailCleanupSettings[]) => Partial<EmailCleanupSettings>)
): EmailCleanupSettings[] => {
  const newPartState: Partial<EmailCleanupSettings> =
    typeof stateUpdate === "function" ? stateUpdate(prevState) : stateUpdate;

  const partPrevState = prevState.find((x) => x.id === newPartState.id);

  if (newPartState == null) {
    return null;
  }

  let stateChanged = false;

  if (prevState == null) {
    stateChanged = newPartState != null;
  } else {
    const objectKeys = Object.keys(newPartState) as Array<
      keyof EmailCleanupSettings
    >;

    stateChanged = objectKeys.some((key) => {
      const equal = isEqual(partPrevState[key], newPartState[key]);

      console.log("Equal:", equal);

      return equal;
    });
  }

  if (stateChanged) {
    const index = prevState.findIndex((x) => x.id === newPartState.id) ?? null;
    const prevStateCopy = [...prevState];

    if (index != null) {
      prevStateCopy[index] = { ...prevStateCopy[index], ...newPartState };

      return prevStateCopy;
    }
  }

  return prevState;
};

export const useEmailCleanerStore = (settings: EmailCleanupSettings[]) => {
  const [state, dispatch] = React.useReducer<
    React.Reducer<EmailCleanupSettings[], Partial<EmailCleanupSettings>>
  >(
    (
      prevState: EmailCleanupSettings[],
      stateUpdate:
        | Partial<EmailCleanupSettings>
        | ((prevState: EmailCleanupSettings[]) => Partial<EmailCleanupSettings>)
    ) => reducer(prevState, stateUpdate),
    settings
  );
  const [settingsIndex, setSettingsIndex] = React.useState<number>(0);

  const [selectedFolderMapping, setSelectedFolderMapping] =
    React.useState<FolderMappingEntry>(
      state[settingsIndex].inboxConfiguration.folderMappings[0]
    );

  const handleUpdateSettingsIndex = React.useCallback((index: number) => {
    setSettingsIndex(index);
  }, []);

  const handleSelectedFolderChanged = React.useCallback(
    (index: number) => {
      setSelectedFolderMapping(
        state[settingsIndex].inboxConfiguration.folderMappings[index]
      );
    },
    [settingsIndex, state]
  );

  const handleUpdateState = React.useCallback(
    (id: number, settings: Partial<EmailCleanupSettings>) => {
      dispatch({ id: id, ...settings });
    },
    [dispatch]
  );

  return {
    settingsIndex,
    selectedFolderMapping,
    emailSettings: state,
    selectedEmailSettings: state[settingsIndex],
    handleSelectedSettingsIndexChanged: handleUpdateSettingsIndex,
    handleSelectedFolderChanged,
    handleUpdateState,
  };
};
