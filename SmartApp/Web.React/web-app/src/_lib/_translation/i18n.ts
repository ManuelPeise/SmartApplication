import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import commonEn from "./resources/common.en.json";
import commonDe from "./resources/common.de.json";
import administrationEn from "./resources/administration.en.json";
import administrationDe from "./resources/administration.de.json";

const resources = {
  en: {
    common: commonEn,
    administration: administrationEn,
  },
  de: {
    common: commonDe,
    administration: administrationDe,
  },
};

i18n.use(initReactI18next).init({
  resources: resources,
  lng: "en",
});
