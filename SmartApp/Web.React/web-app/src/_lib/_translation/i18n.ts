import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import commonEn from "./resources/common.en.json";
import commonDe from "./resources/common.de.json";
import administrationEn from "./resources/administration.en.json";
import administrationDe from "./resources/administration.de.json";
import settingsEn from "./resources/settings.en.json";
import settingsDe from "./resources/settings.de.json";
import emailCleanerEn from "./resources/emailCleaner.en.json";
import emailCleanerDe from "./resources/emailCleaner.de.json";

const resources = {
  en: {
    common: commonEn,
    emailCleaner: emailCleanerEn,
    administration: administrationEn,
    settings: settingsEn,
  },
  de: {
    common: commonDe,
    emailCleaner: emailCleanerDe,
    administration: administrationDe,
    settings: settingsDe,
  },
};

i18n.use(initReactI18next).init({
  resources: resources,
  lng: "en",
  interpolation: {
    escapeValue: false,
  },
});
