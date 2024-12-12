import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import commonEn from "./resources/common.en.json";
import commonDe from "./resources/common.de.json";

const resources = {
  en: {
    common: commonEn,
  },
  de: {
    common: commonDe,
  },
};

i18n.use(initReactI18next).init({
  resources: resources,
  lng: "en",
});
