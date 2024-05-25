import { TextStyle } from "react-native";

interface Colors {
  Neutral: string;
  NeutralContrast: string;
  Primary: string;
  PrimaryContrast: string;
}

const colors: Readonly<Colors> = {
  Neutral: '#FFFFFF',
  NeutralContrast: '#5E5C72',
  Primary: '#4C4A62',
  PrimaryContrast: '#DAD8D8',
};

const contrastTextStyle : Readonly<TextStyle> = {
  color: '#DAD8D8',
  fontSize: 35,
  textShadowColor: '#0149BF',
  textShadowOffset: {width: -1, height: 1},
  textShadowRadius: 10,
}

export { colors, contrastTextStyle };