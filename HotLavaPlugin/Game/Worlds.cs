using HotLavaPlugin.Enums;
using System.Collections.Generic;

namespace HotLavaPlugin.Game
{
    internal static class Worlds
    {
        public static World GymClass => new World("Gym Class", [
            new Course("Gym Jam", [
                new Star("8e72934736772e94eaa50290f7ae341a", "Complete the course"),
                new Star("2c2765d96036f40438807ce21e92b229", "Complete in under 08:00"),
                new Star("2b6db09b813d3f54cb1424cbf312ca8d", "Complete in under 02:10"),
                new Star("e7991396471eede4d82f90334472ae2d", "Reach a speed of 9"),
                new Star("2f492a67f28d82c429c93fa9e48bfe3e", "No Deaths"),
                new Star("718fac78ec94caa478f347b892adae7a", "Grab the golden pin"),
                new Star("7680ae1ce1a1c1f49afc80c4e50df8a1", "Buddy Mode"),
            ]),
            new Course("Trampoline Trouble", [
                new Star("52813bff26455a34c92918c183c034fe", "Complete the course"),
                new Star("181644ae625180e4eb6989129d83e2fb", "Complete in under 02:30"),
                new Star("3262cf7f58754504dbb378912ea87f07", "Complete in under 01:00"),
                new Star("0059648cfcaf1d3459f89b8b76db5eef", "Don't perform any swings"),
                new Star("5d26c3d1697052c45a3f29ab54d8bb0d", "No Deaths"),
                new Star("1a2ff5486c095154c9bf507773051774", "Grab the golden pin"),
                new Star("540b0bdc06ed2dd4887deff906e0575f", "Buddy Mode"),
            ]),
            new Course("Livin' on the Ledge", [
                new Star("f2aca0ab34ef5a34fb9b3d9a52087aff", "Complete the course"),
                new Star("c689392b82845f948b1a088de15c27ab", "Complete in under 02:00"),
                new Star("513a99591da7c8f49855898242e338e7", "Complete in under 00:55"),
                new Star("dc67bb49b6bdd3f4ba6af59e7f46dc7e", "Spend less than 25 seconds on the ground"),
                new Star("df11767c8cafed7429faabbe863fac18", "No Deaths"),
                new Star("3a0ab619a819e514cb7aa6c7ceaa3175", "Grab the golden pin"),
                new Star("2b821237eb81a37448942997afb21e9e", "Buddy Mode"),
            ]),
            new Course("Surfing Surfaces", [
                new Star("b772330c648bc5b4dbf560e1636a765b", "Complete the course"),
                new Star("720fa9c239a1b224eae03b1dd031c9c5", "Complete in under 01:50"),
                new Star("4218afbc7f4895141a4ebd26d61272d3", "Complete in under 00:50"),
                new Star("a6179cec5a395964a807d262ec059a9e", "Reach a speed of 10.5"),
                new Star("5bd4d45837fdac649965d0e2b89e63d7", "No Deaths"),
                new Star("7ed7e2810fd00a14aaea4f267e33400e", "Grab the golden pin"),
                new Star("d2a264a37b85ab045a885dc98fa2ad56", "Buddy Mode"),
            ]),
            new Course("Pole Vault", [
                new Star("b854fd83bb9c6b2438d05cfdaba66a91", "Complete the course"),
                new Star("3bb1fbe4b84c03a4086c6ac366f2ba27", "Complete in under 02:30"),
                new Star("f89d1fb8ad92a634d8703d0b8bb1c976", "Complete in under 01:10"),
                new Star("6bc239d7f01e9ee40a4ce3687f205482", "Spend less than 30 seconds on the ground"),
                new Star("f8a55461338a712468a59dcd64888350", "No Deaths"),
                new Star("5101c9e9015f4204bb42649a58127217", "Grab the golden pin"),
                new Star("b6fb416d4c5c4984db326bab8e23079a", "Buddy Mode"),
            ]),
            new Course("Chase Your Sister", [
                new Star("39d3e311aa4b8dc4e92da8ec2fc093e1", "Complete the course"),
                new Star("46a134cc36e348d4eb3416dc4f9df9de", "Complete in under 01:10"),
                new Star("f6a106bb664df5a4089b4fa5101f0152", "Complete in under 00:45"),
                new Star("34f041456579a904a98c746ea6e6bb2e", "Don't perform any swings"),
                new Star("34a0bf50ab8394f46a9a94542fbfc553", "Tag your sister"),
                new Star("a45613d4c2daadb4aa581548048d2c7a", "Grab the golden pin"),
                new Star("078d212bcd4e55a4f9fe32de2cf0ad2b", "Buddy Mode"), //TODO: should this be treated as a separate course/trial?
            ]),
            new Course("Pogo Trial", CourseType.Pogo, [
                new Star("5069ece9ed4a8ed479d1743620a34023", "Find all the checkpoints using the Pogo Stick"),
            ]),
            new Course("Tiny Toy Trial", CourseType.TinyToy, [
                new Star("98310f580476ff04384f6abd4357370e", "Get to the finish line"),
            ]),
            new Course("Jetpack Trial", CourseType.Jetpack, [
                new Star("62137bb4c16995d4aaf2f385225156e8", "Find all the checkpoints using the Jetpack"),
            ]),
            new Course("All Course Marathon", CourseType.AllCourseMarathon, [
                new Star("e9ee84430ecf11740beb41326451d414", "Complete the course"),
            ]),
        ]);

        public static IEnumerable<World> AllWorlds => [
            GymClass
        ];
    }
}
