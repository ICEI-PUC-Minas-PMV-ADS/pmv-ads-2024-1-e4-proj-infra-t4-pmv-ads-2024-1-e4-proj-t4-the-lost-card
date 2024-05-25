import { createNativeStackNavigator } from '@react-navigation/native-stack';
import DefaultAnnonPage from '../Pages/DefaultAnnonPage';
// import SignInPage from '../Pages/SignInPage';
 import SplashPage from '../Pages/SplashPage';
 import HomePage from '../Pages/HomePage';
// import SignUpPage from '../Pages/SignUpPage';

const {Screen, Navigator} = createNativeStackNavigator();

const AnonRoutes = () => {
    return (
        <Navigator screenOptions={{ headerShown: false }}initialRouteName='SplashPage'>
            {/* <Screen name="DefaultAnnon" component={DefaultAnnonPage} />  */}
            <Screen name="SplashPage" component={SplashPage} />
            <Screen name="HomePage" component={HomePage} />
            {/* <Screen name="SignInPage" component={SignInPage} />
            <Screen name="SingUpPage" component={SignUpPage} /> */}
        </Navigator>
    )
}

export default AnonRoutes;