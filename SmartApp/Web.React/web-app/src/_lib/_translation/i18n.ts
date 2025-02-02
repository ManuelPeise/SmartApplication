import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import commonEn from "./resources/common.en.json";
import commonDe from "./resources/common.de.json";
import administrationEn from "./resources/administration.en.json";
import administrationDe from "./resources/administration.de.json";
import settingsEn from "./resources/settings.en.json";
import settingsDe from "./resources/settings.de.json";
import interfaceEn from "./resources/interface.en.json";
import interfaceDe from "./resources/interface.de.json";

const resources = {
  en: {
    common: commonEn,
    administration: administrationEn,
    settings: settingsEn,
    interface: interfaceEn,
  },
  de: {
    common: commonDe,
    administration: administrationDe,
    settings: settingsDe,
    interface: interfaceDe,
  },
};

i18n.use(initReactI18next).init({
  resources: resources,
  lng: "en",
  interpolation: {
    escapeValue: false,
  },
});
