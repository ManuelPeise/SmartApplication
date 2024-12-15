import React from "react";
import "./_lib/_translation/i18n";
import "./App.css";
import RootLayout from "./_components/_containers/RootLayout";
import AppRouter from "./_lib/AppRouter";
import AuthContextProvider from "./_providers/AuthContextProvider";

const App: React.FC = () => {
  return (
    <RootLayout>
      <AuthContextProvider>
        <AppRouter />
      </AuthContextProvider>
    </RootLayout>
  );
};

export default App;
