import React from "react";
import "./_lib/_translation/i18n";
import "./App.css";
import { useAuth } from "./_hooks/useAuth";
import { UserRoleEnum } from "./_lib/_enums/UserRoleEnum";
import AuthenticationPage from "./_Stacks/_AuthStack/AuthenticationPage";
import PageContainer from "./_components/_containers/PageContainer";

const App: React.FC = () => {
  const { authenticationState } = useAuth();

  console.log("Userrole", authenticationState);
  return (
    <PageContainer>
      {!authenticationState ? (
        <AuthenticationPage />
      ) : authenticationState.jwtData.userRole === UserRoleEnum.Admin ? (
        <div>Admin page</div>
      ) : (
        <div>User Page</div>
      )}
    </PageContainer>
  );
};

export default App;
