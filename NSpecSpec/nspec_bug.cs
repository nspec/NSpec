using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec.Interpreter.Indexer;
using NSpec.Extensions;

namespace NSpecSpec
{
    /*
    works in rspec: 
    describe RSpec do
        before(:each) { @array = Array.new }

        context "something that works in rspec but not nspec" do
            context "sibling context" do
                before(:each) { @array << "sibling 1" }

                it { @array.count.should == 1 }
            end

            context "another sibling context" do
                before(:each) { @array << "sibling 2" }

                it { @array.count.should == 1 }
            end
        end
    end    
    */

    public class nspec_bug : spec                                          
    {
        private List<int> ints = null;

        public void describe_NSpec()                                       //describe RSpec do
        {
            before.each = () => ints = new List<int>();                    //  before(:each) { @array = Array.new }

            when["something that works in rspec but not nspec"] = () =>    //  context "something that works in rspec but not nspec" do
            {
                before.each = () => ints.Add(1);

                given["sibling context"] = () =>                           //    context "sibling context" do
                {
                    before.each = () => ints.Add(1);                       //      before(:each) { @array << "sibling 1" }

                    specify(() => ints.Count.should_be(1));                //        it { @array.count.should == 1 }
                };                                                         //    end

                given["another sibling context"] = () =>                   //    context "another sibling context" do
                {
                    before.each = () => ints.Add(1);                       //      before(:each) { @array << "sibling 2" }

                    specify(() => ints.Count.should_be(1));                //      it { @array.count.should == 1 }
                };                                                         //    end
            };                                                             //  end
        }                                                                  //end
    }
}
